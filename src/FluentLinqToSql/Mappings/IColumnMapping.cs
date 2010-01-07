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
	using System.Data.Linq.Mapping;

	/// <summary>
	/// Defines a mapped property. 
	/// </summary>
	public interface IColumnMapping : IPropertyMapping {
		/// <summary>
		/// Sets the name of the database column to be mapped. When omitted, the property name will be used.
		/// </summary>
		/// <param name="name">Name of database column</param>
		/// <returns></returns>
		IColumnMapping Named(string name);

		/// <summary>
		/// Specifies that Linq to Sql should bypass the property and instead access the specified private field.
		/// </summary>
		/// <param name="fieldName">Name of field to be used for storage</param>
		/// <returns></returns>
		IColumnMapping Storage(string fieldName);

		/// <summary>
		/// Specifies the AutoSync value for the column. 
		/// </summary>
		/// <param name="autoSync">The autosync setting for the column</param>
		/// <returns></returns>
		IColumnMapping AutoSync(AutoSync autoSync);

		/// <summary>
		/// Specifies that the column is not nullable.
		/// </summary>
		/// <returns></returns>
		IColumnMapping NotNull();

		/// <summary>
		/// Specifies the database type that should be used when generating the schema.
		/// </summary>
		/// <param name="dbType">Database type</param>
		/// <returns></returns>
		IColumnMapping DbType(string dbType);

		/// <summary>
		/// Specifies that the following SQL should be executed to calculate the value for the specified field.
		/// </summary>
		/// <param name="expression">The expression to use</param>
		/// <returns></returns>
		IColumnMapping Expression(string expression);

		/// <summary>
		/// Specifies that the column is a primary key.
		/// </summary>
		/// <returns></returns>
		IColumnMapping PrimaryKey();

		/// <summary>
		/// Specifies that the column is a version/timestamp field
		/// </summary>
		/// <returns></returns>
		IColumnMapping Version();

		/// <summary>
		/// Specifies the UpdateCheck value to use when saving changes. 
		/// </summary>
		/// <param name="updateCheck">The UpdateCheck value to use</param>
		/// <returns></returns>
		IColumnMapping UpdateCheck(UpdateCheck updateCheck);

		/// <summary>
		/// Specifies that the value of the current column is generated in the database.
		/// </summary>
		/// <returns></returns>
		IColumnMapping DbGenerated();
	}
}