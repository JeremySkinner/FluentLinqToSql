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

namespace FluentLinqToSql.Tests {
	using System;
	using System.Xml.Linq;
	using NUnit.Framework;

	public static class TestExtensions {
		private static readonly XNamespace xmlNamespace = "http://schemas.microsoft.com/linqtosql/mapping/2007";

		public static XElement ShouldHaveElement(this XContainer parent, string name) {
			var elem = parent.Element(xmlNamespace + name);
			if (elem == null) {
				throw new AssertionException(string.Format("Could not find an element named {0}", name));
			}

			return elem;
		}

		public static XElement ShouldHaveAttribute(this XElement parent, string name, string value) {
			var attr = parent.Attribute(name);
			if (attr == null) {
				throw new AssertionException(string.Format("Could not find an attribute named {0}", name));
			}

			if (attr.Value != value) {
				throw new AssertionException(string.Format("Expected attribute '{0}' to have a value of '{1}'. Actual was '{2}' ", name, value, attr.Value));
			}
			return parent;
		}

		public static XElement ShouldBeNamed(this XElement element, string name) {
			var realName = xmlNamespace + name;
			if(element.Name != realName) {
				throw new AssertionException(string.Format("Expected element to have the name '{0}'. Actual was '{1}", realName, element.Name));
			}

			return element;
		}

		public static T ShouldEqual<T>(this T item, object value) {
			Assert.That(item, Is.EqualTo(value));
			return item;
		}

		public static T ShouldBeOfType<T>(this object source) {
			Assert.IsInstanceOf<T>(source);
			return (T)source;
		}

		public static T CastTo<T>(this object objtoCast) {
			if (objtoCast is T) {
				return (T)objtoCast;
			}

			throw new Exception("Could not cast object - incorrect type");
		}

		public static void ShouldBeFalse(this bool b) {
			Assert.IsFalse(b);
		}

		public static void ShouldBeTrue(this bool b) {
			Assert.IsTrue(b);
		}

		public static void ShouldBeNull(this object source) {
			Assert.IsNull(source);
		}

	}
}