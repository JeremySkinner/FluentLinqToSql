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
	using System.Collections.Generic;
	using System.Data.Linq.Mapping;
	using System.Linq;
	using System.Reflection;
	using System.Xml.Linq;
	using Internal;

	/// <summary>
	/// Mapping class that represents a property mapped to a column.
	/// </summary>
	public class ColumnMapping : IColumnMapping {
		private readonly IDictionary<string, string> attributes = new Dictionary<string, string>();
		private readonly IDictionary<string, object> customProperties = new Dictionary<string, object>();

		/// <summary>
		/// The property that is mapped to the database.
		/// </summary>
		public MemberInfo Property { get; private set; }

		/// <summary>
		/// Anu additional custom properties that are associated with the mapping.
		/// </summary>
		public IDictionary<string, object> CustomProperties {
			get { return customProperties; }
		}

		/// <summary>
		/// Attributes that will be converted into xml
		/// </summary>
		public IDictionary<string, string> Attributes {
			get { return attributes; }
		}

		/// <summary>
		/// Creates a new instance of the ColumnMapping class.
		/// </summary>
		/// <param name="property">The property that should be mapped</param>
		public ColumnMapping(MemberInfo property) {
			Property = property;
			attributes[Constants.Member] = Property.Name;
		}

		/// <summary>
		/// Specifies the name of the column to which the property should be mapped.
		/// </summary>
		/// <param name="name">The name</param>
		/// <returns></returns>
		public IColumnMapping Named(string name) {
			name.Guard("A name must be specified when calling 'Named'");
			return SetAttribute(Constants.Name, name);
		}

		/// <summary>
		/// Sets an attribute that will be generated in the XML.
		/// </summary>
		/// <param name="name">Name of attribute</param>
		/// <param name="value">value</param>
		/// <returns></returns>
		public IColumnMapping SetAttribute(string name, string value) {
			attributes[name] = value;
			return this;
		}

		/// <summary>
		/// Specifies that Linq to Sql should bypass the public property and use the specified field directly.
		/// </summary>
		/// <param name="fieldName">Name of field that the column should be mapped to</param>
		/// <returns></returns>
		public IColumnMapping Storage(string fieldName) {
			return SetAttribute(Constants.Storage, fieldName);
		}

		/// <summary>
		/// Specifies the AytoSync value for this property. 
		/// AutoSync instructs the runtime how to retrieve the value after an insert or update operation.
		/// </summary>
		/// <param name="autoSync">The autosync value</param>
		/// <returns></returns>
		public IColumnMapping AutoSync(AutoSync autoSync) {
			return SetAttribute(Constants.AutoSync, autoSync.ToString());
		}

		/// <summary>
		/// Specifies that the column is not nullable.
		/// </summary>
		/// <returns></returns>
		public IColumnMapping NotNull() {
			return SetAttribute(Constants.CanBeNull, "false");
		}

		/// <summary>
		/// Specifies the type of the column in the database.
		/// </summary>
		/// <param name="dbType">Data type for the column</param>
		/// <returns></returns>
		public IColumnMapping DbType(string dbType) {
			return SetAttribute(Constants.DbType, dbType);
		}

		/// <summary>
		/// Specifies the expression that should be used to calculate the column.
		/// </summary>
		/// <param name="expression">The expression that should be used to calculate the column</param>
		/// <returns></returns>
		public IColumnMapping Expression(string expression) {
			return SetAttribute(Constants.Expression, expression);
		}

		/// <summary>
		/// Marks the column as part of the table's Primary Key.
		/// </summary>
		/// <returns></returns>
		public IColumnMapping PrimaryKey() {
			return SetAttribute(Constants.IsPrimaryKey, "true");
		}

		/// <summary>
		/// Marks the column as a database timestamp or version number.
		/// </summary>
		/// <returns></returns>
		public IColumnMapping Version() {
			return SetAttribute(Constants.IsVersion, "true");
		}

		/// <summary>
		/// Specifies how Linq to Sql approaches the detection of optimistic concurrency conflicts. 
		/// </summary>
		/// <remarks>
		///	The default is 'Always' unless the entity has a version field is defined.
		/// </remarks>
		/// <param name="updateCheck">The UpdateCheck value to use</param>
		/// <returns></returns>
		public IColumnMapping UpdateCheck(UpdateCheck updateCheck) {
			return SetAttribute(Constants.UpdateCheck, updateCheck.ToString());
		}

		/// <summary>
		/// Marks the column as generated by the database.
		/// </summary>
		/// <returns></returns>
		public IColumnMapping DbGenerated() {
			return SetAttribute(Constants.IsDbGenerated, "true");
		}

		/// <summary>
		/// Convers the column mapping to an Column element.
		/// </summary>
		/// <returns>XElement</returns>
		public IEnumerable<XElement> ToXml() {

			//if no explict name has been specified, use the name of the property.
			if(! attributes.ContainsKey(Constants.Name)) {
				SetAttribute(Constants.Name, Property.Name);
			}

			yield return new LinqElement(Constants.Column,
			                             attributes.Select(attribute => new XAttribute(attribute.Key, attribute.Value))
				);
		}
	}
}