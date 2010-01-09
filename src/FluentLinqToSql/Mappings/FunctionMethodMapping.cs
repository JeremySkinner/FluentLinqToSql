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
	using System.Reflection;
	using System.Xml.Linq;
	using Internal;

	/// <summary>
	/// Represents a method that is mapped to a Stored Procedure or User Defined Function.
	/// </summary>
	public class FunctionMethodMapping : IFunctionMethodMapping {
		private readonly MethodInfo method;
		private readonly Dictionary<string, string> attributes = new Dictionary<string, string>();
		private readonly Dictionary<string, object> customProperties = new Dictionary<string, object>();
		private readonly FunctionParameterMapping[] parameters;
		private readonly List<IMapping> elementTypes = new List<IMapping>();

		public FunctionMethodMapping(MethodInfo method, FunctionParameterMapping[] parameters) {
			this.method = method;
			attributes[Constants.Name] = method.Name;
			attributes[Constants.Method] = method.Name;
			this.parameters = parameters;
		}

		public MethodInfo Method {
			get { return method; }
		}

		public IDictionary<string, object> CustomProperties {
			get { return customProperties; }
		}

		public IDictionary<string, string> Attributes {
			get { return attributes; }
		}

		public IFunctionMethodMapping SetAttribute(string name, string value) {
			attributes[name] = value;
			return this;
		}

		public IFunctionMethodMapping Composable() {
			return SetAttribute(Constants.IsComposable, "true");
		}

		public IFunctionMethodMapping ElementType<T>() {
			return ElementType<T>(null);
		}

		public IFunctionMethodMapping ElementType<T>(Action<FunctionReturnTypeMapping<T>> action) {
			var mapping = new FunctionReturnTypeMapping<T>();

			if(action != null) {
				action(mapping);
			}

			elementTypes.Add(mapping);
			return this;
		}

		public IFunctionMethodMapping Named(string name) {
			return SetAttribute(Constants.Name, name);
		}

		public IEnumerable<XElement> ToXml() {
			yield return new LinqElement(Constants.Function,
                 attributes.Select(attribute => new XAttribute(attribute.Key, attribute.Value)),
                 parameters.Select(param => param.ToXml()),
				 elementTypes.Select(e => e.ToXml().Single())
			);
		}
	}
}