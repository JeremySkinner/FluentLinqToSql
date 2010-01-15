#region License
// Copyright 2008 Jeremy Skinner (http://www.jeremyskinner.co.uk)
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://www.codeplex.com/FluentLinqToSql
#endregion

namespace FluentLinqToSql.TestHelper {
	using System;
	using System.Collections.Generic;
	using System.Data.Linq;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using Internal;

	//This file uses some code from Fluent NHibernate's PersistenceSpecification class:
	//http://fluent-nhibernate.googlecode.com/svn/trunk/src/FluentNHibernate.Framework/PersistenceSpecification.cs

	/// <summary>
	/// Used in unit tests to verify that a mapping is correct.
	/// </summary>
	/// <typeparam name="T">Type of entity</typeparam>
	public class MappingTester<T> : IDisposable where T : class, new() {
		private readonly List<IPropertyValue<T>> propertyValues = new List<IPropertyValue<T>>();
		private readonly Func<DataContext> dataContextFactory;
		private DataContext currentContext;
		public DataLoadOptions LoadOptions { get; private set; }


		/// <summary>
		/// Creates a new instance of the MappingTester class.
		/// </summary>
		/// <param name="dataContextFactory">Delegate used to create the datacontext necessary for testing.</param>
		public MappingTester(Func<DataContext> dataContextFactory) {
			this.dataContextFactory = dataContextFactory;
			this.currentContext = dataContextFactory();
			LoadOptions = new DataLoadOptions();
		}

		/// <summary>
		/// Tests the mapping of a particular property
		/// </summary>
		/// <param name="propertyExpression">Lambda that specifies the property to test</param>
		/// <param name="value">The value that should be set on the property</param>
		public MappingTester<T> TestProperty<TProperty>(Expression<Func<T, TProperty>> propertyExpression, TProperty value) {
			var property = propertyExpression.GetMember();
			propertyValues.Add(new PropertyValue<T>(property, value));
			return this;
		}

		/// <summary>
		/// Tests the mapping of a particular collection
		/// </summary>
		/// <param name="propertyExpression">Lambda that specifies the collection property to test</param>
		/// <param name="values">Items that should go in the collection</param>
		public MappingTester<T> TestCollection<TElement>(Expression<Func<T, IList<TElement>>> propertyExpression, params TElement[] values) where TElement : class {
			var property = propertyExpression.GetMember();
			propertyValues.Add(new ListPropertyValue<T, TElement>(property, values));
			return this;
		}

		/// <summary>
		/// Disposes of the mapping tester and verifies all expectations.
		/// </summary>
		public void Dispose() {
			Verify();
		}


		private ITable GetTableForType(Type type) {
			var metaType = currentContext.Mapping.GetMetaType(type);
			if(metaType.InheritanceBase != null) {
				return currentContext.GetTable(metaType.InheritanceBase.Type);
			}

			return currentContext.GetTable(type);
		}

		/// <summary>
		/// Verifies that all properties have been mapped successfully.
		/// </summary>
		public T Verify() {
			var first = new T();
			var table = GetTableForType(typeof(T));

			propertyValues.ForEach(prop=>prop.SetValue(first));

			table.InsertOnSubmit(first);

			currentContext.SubmitChanges();
			currentContext.Dispose();

			//Recreate the datacontext to ensure we don't get the same instance back.
			currentContext = dataContextFactory();

			if(LoadOptions != null) {
				currentContext.LoadOptions = LoadOptions;
			}

			var second = GetTableForType(typeof(T)).OfType<T>().Single(x => x == first);
			
			System.Diagnostics.Debug.Assert(!ReferenceEquals(first, second));

			propertyValues.ForEach(prop => prop.Verify(second));

			return second;
		}

		private interface IPropertyValue<TEntity> {
			void Verify(TEntity entity);
			void SetValue(TEntity entity);
		}

		private class PropertyValue<TEntity> : IPropertyValue<TEntity> {
			private MemberInfo property;
			private object expected;

			public PropertyValue(MemberInfo property, object value) {
				this.property = property;
				this.expected = value;
			}

			public void Verify(TEntity entity) {
				var value = property.GetValue(entity);
				if(! expected.Equals(value)) {
					throw new MappingException(string.Format("Expected '{0}' for property {1}. Actual was: '{2}'", expected, property.Name, value));
				}
			}

			public void SetValue(TEntity entity) {
				property.SetValue(entity, expected);
			}
		}

		private class ListPropertyValue<TEntity, TElement> : IPropertyValue<TEntity> {
			private MemberInfo property;
			private IEnumerable<TElement> expectedValues;

			public ListPropertyValue(MemberInfo property, IEnumerable<TElement> list) {
				this.property = property;
				this.expectedValues = list;
			}


			public void Verify(TEntity entity) {
				var values = property.GetValue(entity) as IEnumerable<TElement>;
				
				if(values == null) {
					throw new MappingException(string.Format("Expected a collection of {0} objects for property {1}.", typeof(TElement).Name, property.Name));
				}

				int actualCount = values.Count();
				int expectedCount = expectedValues.Count();

				if(values.Count() != expectedValues.Count()) {
					throw new MappingException(string.Format("Expected {0} items for property {1}. Actual was {2}", expectedCount, property.Name, actualCount));
				}
			}

			public void SetValue(TEntity entity) {
				var list = property.GetValue(entity) as IList<TElement>; 
				if(list == null) {
					throw new MappingException(string.Format("Expected property {0} on type {1} to be a non-null IList<{2}>", property.Name, typeof(TEntity).Name, typeof(TElement).Name));
				}

				expectedValues.ForEach(list.Add);
			}
		}
	}
}