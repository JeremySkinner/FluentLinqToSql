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
	using System.Linq;
	using System.Reflection;
	using System.Xml.Linq;
	using Internal;

	public class FunctionParameterMapping : IFunctionParameterMapping {
		private readonly IDictionary<string, string> attributes = new Dictionary<string, string>();
		private readonly IDictionary<string, object> customProperties = new Dictionary<string, object>();

		public FunctionParameterMapping(ParameterInfo parameter) {
			Parameter = parameter;
			attributes["Parameter"] = parameter.Name;
		}

		public ParameterInfo Parameter { get; private set; }

		public IDictionary<string, string> Attributes {
			get { return attributes; }
		}

		public IDictionary<string, object> CustomProperties {
			get { return customProperties; }
		}

		public IEnumerable<XElement> ToXml() {
			yield return new LinqElement("Parameter",
			                             Attributes.Select(attribute => new XAttribute(attribute.Key, attribute.Value))
				);
		}

		public IFunctionParameterMapping SetAttribute(string key, string value) {
			attributes[key] = value;
			return this;
		}

		public IFunctionParameterMapping DbType(string dbType) {
			return SetAttribute("DbType", dbType);
		}

		public IFunctionParameterMapping Named(string name) {
			return SetAttribute("Name", name);
		}
	}
}