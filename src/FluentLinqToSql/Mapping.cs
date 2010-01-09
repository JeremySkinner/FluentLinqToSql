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
	using System.Collections.Generic;
	using System.Linq;
	using System.Xml.Linq;
	using Internal;
	using Mappings;

	/// <summary>
	/// Defines the mapping for an entity.
	/// </summary>
	/// <typeparam name="T">Type of entity being mapped.</typeparam>
	public class Mapping<T> : TypeMapping<T> {
		
		/// <summary>
		/// Creats a new instance of the Mapping class.
		/// </summary>
		public Mapping() {
			TableName = MappedType.Name;
		}

		private string TableName {
			get { return CustomProperties[Constants.TableName].ToString(); }
			set { CustomProperties[Constants.TableName] = value; }
		}
		

		/// <summary>
		/// Sets the name of the table. By default, the name of the class will be used.
		/// </summary>
		/// <param name="tableName">Name of table to map to</param>
		public void Named(string tableName) {
			tableName.Guard("A table name must be specified when calling 'Named'");
			TableName = tableName;
		}

		/// <summary>
		/// Produces an XML element that can be used to construct the mapping document.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<XElement> ToXml() {
			var typeElement = base.ToXml().Single();

			yield return new LinqElement(
				Constants.Table,
				new XAttribute(Constants.Name, TableName),
				typeElement
			);
		}
	}
}