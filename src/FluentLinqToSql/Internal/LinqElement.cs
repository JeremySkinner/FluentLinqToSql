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

namespace FluentLinqToSql.Internal {
	using System.Xml.Linq;

	/// <summary>
	/// Represents an XML Element within the Linq-to-sql namespace 
	/// </summary>
	public class LinqElement : XElement {
		private static readonly XNamespace xmlNamespace = "http://schemas.microsoft.com/linqtosql/mapping/2007";

		/// <summary>
		/// Creates a new instance of the LinqElement class.
		/// </summary>
		/// <param name="name">Element name</param>
		/// <param name="content">Element content</param>
		public LinqElement(string name, params object[] content) : base(xmlNamespace + name, content) { }
	}
}