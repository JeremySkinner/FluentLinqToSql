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
	using System.Collections.Generic;
	using System.Data.Linq.Mapping;
	using System.Reflection;
	using System.Xml.Linq;
	using Mappings;
	using Modifications;
	using NUnit.Framework;

	[TestFixture]
	public class ReplacePascalCaseWithUnderscoreModificationTester {
		private IMappingModification modification;

		[SetUp]
		public void Setup() {
			modification = new ReplacePascalCaseWithUnderscoreModification();
		}

		[Test]
		public void Should_replace_pascal_cased_column() {
			var mapping = new TestMapping();
			var columnMapping = new ColumnMapping(typeof(Customer).GetProperty("DateOfBirth"));
			mapping.Mappings.Add(columnMapping);
			modification.ApplyTo(mapping);

			Assert.That(columnMapping.Attributes["Name"], Is.EqualTo("Date_Of_Birth"));
		}

		[Test]
		public void Should_not_replace_column_with_explict_name() {
			var mapping = new TestMapping();
			var columnMapping = new ColumnMapping(typeof(Customer).GetProperty("DateOfBirth"));
			columnMapping.Named("Foo");

			mapping.Mappings.Add(columnMapping);
			modification.ApplyTo(mapping);

			Assert.That(columnMapping.Attributes["Name"], Is.EqualTo("Foo"));
		}


		private class TestMapping : IMapping {
			private readonly List<IElementMapping> mappings = new List<IElementMapping>();

			public Type MappedType {
				get { return null; }
			}

			public string Name {
				get { throw new NotImplementedException(); }
			}

			public IEnumerable<XElement> ToXml() {
				throw new NotImplementedException();
			}

			public IList<IElementMapping> Mappings {
				get { return mappings; }
			}

			public IDictionary<string, object> CustomProperties {
				get { throw new NotImplementedException(); }
			}
		}
	}
}