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
	using System.Linq.Expressions;
	using Modifications;

	/// <summary>
	/// Defines a mapping that represents an association between two entities
	/// </summary>
	public interface IAssociationMapping<T, TElement> {
		/// <summary>
		/// Defines an attribute on the property mapping.
		/// </summary>
		/// <param name="name">Attribute name</param>
		/// <param name="value">Attribute value</param>
		/// <returns></returns>
		IAssociationMapping<T, TElement> SetAttribute(string name, string value);
		/// <summary>
		/// Sets the name of the foreign key that will be used when creating the database schema. 
		/// </summary>
		/// <param name="name">Name of foreign key</param>
		/// <returns></returns>
		IAssociationMapping<T, TElement> ForeignKeyName(string name);
		/// <summary>
		/// Sets a private storage field to hold the value for the association.
		/// </summary>
		/// <remarks>
		/// Be sure to correctly set the storage if you are using the EntitySet/EntityRef classes for associations. 
		/// The storage can be determined automatically by using the <see cref="UseFieldForAssociationStorage">UseFieldForAssociationStorage modification</see>
		/// </remarks>
		/// <param name="fieldName">Name of the field that should be used as storage</param>
		/// <returns></returns>
		IAssociationMapping<T, TElement> Storage(string fieldName);
		/// <summary>
		/// Sets the delete behaviour for the association.
		/// </summary>
		/// <param name="deleteRule">String representing the rule</param>
		/// <returns></returns>
		IAssociationMapping<T, TElement> DeleteRule(string deleteRule);
		/// <summary>
		/// Sets the property of this entity class to represent the key value on this side of the association. 
		/// </summary>
		/// <remarks>
		/// If ThisKey is omitted, the primary key fields for this entity will be used.
		/// </remarks>
		/// <param name="keyName">Name of the property.</param>
		/// <returns></returns>
		IAssociationMapping<T, TElement> ThisKey(string keyName);
		/// <summary>
		/// Sets the property of the target entity class that should act as key values on the other side of the association.
		/// </summary>
		/// <remarks>
		/// If OtherKey is omitted, the primary key fields for the related entity will be used.
		/// </remarks>
		/// <param name="keyName">Name of the property</param>
		/// <returns></returns>
		IAssociationMapping<T, TElement> OtherKey(string keyName);
		/// <summary>
		/// Sets the property of the target entity class that should act as key values on the other side of the association.
		/// </summary>
		/// <remarks>
		/// If OtherKey is omitted, the primary key fields for the related entity will be used.
		/// </remarks>
		/// <param name="keyExpression">Lambda expression that specifies the property</param>
		/// <returns></returns>
		IAssociationMapping<T, TElement> OtherKey(Expression<Func<TElement, object>> keyExpression);
		/// <summary>
		/// Sets the property of this entity class to represent the key value on this side of the association. 
		/// </summary>
		/// <remarks>
		/// If ThisKey is omitted, the primary key fields for this entity will be used.
		/// </remarks>
		/// <param name="keyExpression">Lambda expression that specifies the property</param>
		/// <returns></returns>
		IAssociationMapping<T, TElement> ThisKey(Expression<Func<T, object>> keyExpression);
	}
}