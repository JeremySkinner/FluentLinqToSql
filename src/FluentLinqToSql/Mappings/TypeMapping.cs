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

namespace FluentLinqToSql.Mappings {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Xml.Linq;
	using Internal;

	/// <summary>
	/// Defines a mapped class and generates a 'Type' element.
	/// </summary>
	/// <typeparam name="T">Type of entity being mapped</typeparam>
	public class TypeMapping<T> : IMapping {
		private readonly List<IElementMapping> columnMappings = new List<IElementMapping>();
		private readonly IDictionary<string, object> customProperties = new Dictionary<string, object>();

		public TypeMapping() {
			TypeName = typeof(T).FullName;
		}

		private string TypeName {
			get { return customProperties[Constants.TypeName].ToString(); }
			set { customProperties[Constants.TypeName] = value; }
		}

		IList<IElementMapping> IMapping.Mappings {
			get { return columnMappings; }
		}

		/// <summary>
		/// Custom properties associated with the mapping.
		/// </summary>
		public IDictionary<string, object> CustomProperties {
			get { return customProperties; }
		}

		/// <summary>
		/// The type of entity being mapped
		/// </summary>
		public Type MappedType {
			get { return typeof(T); }
		}

		/// <summary>
		/// Maps the specified property
		/// </summary>
		/// <typeparam name="TProperty">Type of property</typeparam>
		/// <param name="propertySelector">Lambda expression that specifies the property to map</param>
		/// <returns>IColumnMapping</returns>
		public IColumnMapping Map<TProperty>(Expression<Func<T, TProperty>> propertySelector) {
			propertySelector.Guard("An expression must be specified when calling Map");

			var property = propertySelector.GetMember();

			if (property == null) {
				throw new ArgumentException("You can only pass MemberExpressions to Map.");
			}

			var columnMapping = new ColumnMapping(property);
			columnMappings.Add(columnMapping);
			return columnMapping;
		}

		/// <summary>
		/// Defines an Identity column (shortcut for Map(x => x.Property).PrimaryKey().DbGenerated();
		/// </summary>
		/// <typeparam name="TProperty">Type of property</typeparam>
		/// <param name="propertySelector">Lambda expression that specifies the property to map</param>
		/// <returns></returns>
		public IColumnMapping Identity<TProperty>(Expression<Func<T, TProperty>> propertySelector) {
			return Map(propertySelector).PrimaryKey().DbGenerated();
		}

		/// <summary>
		/// Defines a one-to-many association.
		/// </summary>
		/// <typeparam name="TElement">Type of entity in the related collection</typeparam>
		/// <param name="propertySelector">Lambda expression that specifies the collection property to map</param>
		/// <returns></returns>
		public IAssociationMapping<T, TElement> HasMany<TElement>(Expression<Func<T, IList<TElement>>> propertySelector) {
			propertySelector.Guard("An expression must be specified when calling HasMany");

			var property = propertySelector.GetMember();

			if (property == null) {
				throw new ArgumentException("You can only pass a MemberExpression to HasMany");
			}

			var hasMany = new HasManyMapping<T, TElement>(property);
			columnMappings.Add(hasMany);
			return hasMany;
		}

		/// <summary>
		/// Defines a many-to-one association.
		/// </summary>
		/// <typeparam name="TReference">Type of related entity</typeparam>
		/// <param name="propertySelector">Lambda expression that specifies the property to map</param>
		/// <returns></returns>
		public IAssociationMapping<T, TReference> BelongsTo<TReference>(Expression<Func<T, TReference>> propertySelector) {
			propertySelector.Guard("An expression myst be specified when calling BelongsTo");

			var property = propertySelector.GetMember();

			if (property == null) {
				throw new ArgumentException("You can only pass a MemberExpression to BelongsTo");
			}

			var belongsTo = new BelongsToMapping<T, TReference>(property);
			columnMappings.Add(belongsTo);
			return belongsTo;
		}


		/// <summary>
		/// Defines a true one-to-one association.
		/// </summary>
		/// <typeparam name="TReference">Type of related entity.</typeparam>
		/// <param name="propertySelector">Lambda expression that specifies the property to map.</param>
		/// <returns></returns>
		public IAssociationMapping<T, TReference> HasOne<TReference>(Expression<Func<T, TReference>> propertySelector) {
			propertySelector.Guard("An expression must be specified when calling HasOne");

			var property = propertySelector.GetMember();

			if (property == null) {
				throw new ArgumentException("You can ony pass a MemberExpression to HasOne");
			}

			var hasOne = new HasOneMapping<T, TReference>(property);
			columnMappings.Add(hasOne);
			return hasOne;
		}

		/// <summary>
		/// Marks a property as being a discriminator and allows for the definition of subclasses
		/// </summary>
		/// <typeparam name="TProperty">Type of property</typeparam>
		/// <param name="propertySelector">Expression that specifies property to map</param>
		/// <returns></returns>
		public IDiscriminatorMapping<T, TProperty> DiscriminateOnProperty<TProperty>(Expression<Func<T, TProperty>> propertySelector) {
			propertySelector.Guard("An expression must be specified when calling DiscriminateOnProperty");

			var property = propertySelector.GetMember();
			if(property == null) {
				throw new ArgumentException("You can only pass a MemberExpression to DiscriminateOnProperty");
			}

			var columnMapping = columnMappings.OfType<IColumnMapping>().FirstOrDefault(x => x.Property == property);

			if(columnMapping == null) {
				columnMapping = Map(propertySelector);
			}
			
			columnMapping.Attributes[Constants.IsDiscriminator] = "true";

			var discriminator = new DiscriminatorMapping<T, TProperty>(this);
			columnMappings.Add(discriminator);
			return discriminator;
		}

		/// <summary>
		/// Produces an XML element that can be used to construct the mapping document.
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<XElement> ToXml() {
			var elem  = new LinqElement(
				Constants.Type,
				new XAttribute(Constants.Name, TypeName),
				columnMappings.Select(propertyMapping => propertyMapping.ToXml())
			);

			if(customProperties.ContainsKey(Constants.IsInheritanceDefault)) {
				elem.Add(new XAttribute(Constants.IsInheritanceDefault, customProperties[Constants.IsInheritanceDefault]));
			}

			if(customProperties.ContainsKey(Constants.InheritanceCode)) {
				elem.Add(new XAttribute(Constants.InheritanceCode, customProperties[Constants.InheritanceCode]));
			}

			yield return elem;
		}
	}
}