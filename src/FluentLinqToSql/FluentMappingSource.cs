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

namespace FluentLinqToSql {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Data.Linq.Mapping;
	using System.Linq;
	using System.Reflection;
	using System.Xml.Linq;
	using Internal;
	using Mappings;
	using Modifications;

	/// <summary>
	/// Fluent Mapping source
	/// </summary>
	public class FluentMappingSource : IEnumerable<IMapping>  {
		private readonly Dictionary<Type, IMapping> mappings = new Dictionary<Type, IMapping>();
		private readonly List<IMappingModification> modifications = new List<IMappingModification>();
		private readonly string databaseName;

		/// <summary>
		/// Creates a new instance of the FluentMappingSource object 
		/// </summary>
		/// <param name="databaseName">The name of the database being mapped</param>
		public FluentMappingSource(string databaseName) {
			this.databaseName = databaseName;
		}

		/// <summary>
		/// Creates an XmlMappingSource generated from the fluent mapping information
		/// </summary>
		/// <returns>An XmlMappingSource object</returns>
		public virtual MappingSource CreateMappingSource() {
			return XmlMappingSource.FromXml(CreateDocument().ToString());
		}

		/// <summary>
		/// Creates the XML Document containing the mapping information
		/// </summary>
		/// <returns>XDocument</returns>
		public XDocument CreateDocument() {
			var document = new XDocument(
				new LinqElement("Database", new XAttribute("Name", databaseName))
				);

			document.Root.Add(
				mappings.Select(p => p.Value).Select(mapping => ApplyMappingModifications(mapping).ToXml())
			);

			return document;
		}

		private IMapping ApplyMappingModifications(IMapping mapping) {
			modifications.ForEach(modification => modification.ApplyTo(mapping));
			return mapping;
		}

		/// <summary>
		/// Adds a mapping modification that can be used to modify the mapping before the XML document is constructed.
		/// </summary>
		/// <param name="modification">The mapping modification</param>
		public FluentMappingSource AddModification(IMappingModification modification) {
			modification.Guard("Cannot pass null to 'AddModification'");
			modifications.Add(modification);
			return this;
		}

		/// <summary>
		/// Adds all mappings that can be located in the specified assembly
		/// </summary>
		/// <param name="assembly"></param>
		public virtual FluentMappingSource AddFromAssembly(Assembly assembly) {
			assembly.Guard("Cannot pass null to 'AddFromAssembly'");

			var mappingObjects = from type in assembly.GetExportedTypes()
			                     where typeof(IMapping).IsAssignableFrom(type)
			                     let mapping = InstantiateMapping(type)
			                     select new {Type = mapping.MappedType, Mapping = mapping};


			mappingObjects.ForEach(x => mappings.Add(x.Type, x.Mapping));
			return this;
		}

		protected virtual IMapping InstantiateMapping(Type mappingType) {
			return (IMapping)Activator.CreateInstance(mappingType);
		}

		/// <summary>
		/// Adds all mappings from the assembly containing the specified type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public virtual FluentMappingSource AddFromAssemblyContaining<T>() {
			return AddFromAssembly(typeof(T).Assembly);
		}

		/// <summary>
		/// Adds a mapping class to the mapping source
		/// </summary>
		/// <param name="mapping">The mapping to add to the mapping source</param>
		public FluentMappingSource AddMapping(IMapping mapping) {
			mappings.Add(mapping.MappedType, mapping);
			return this;
		}

		/// <summary>
		/// Returns an enumerator of IMapping objects that iterates through the collection.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<IMapping> GetEnumerator() {
			return mappings.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		/// <summary>
		/// Gets the mapping for the specified type.
		/// </summary>
		/// <param name="entityType">Type of entity</param>
		/// <returns>Mapping</returns>
		public IMapping this[Type entityType] {
			get { return mappings[entityType]; }
		}

		public static implicit operator MappingSource(FluentMappingSource fluentMappingSource) {
			return fluentMappingSource.CreateMappingSource();
		}
	}
}