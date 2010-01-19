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
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Linq;
	using Mappings;
	using Modifications;
	using NUnit.Framework;

	[TestFixture]
	public class MappingSourceTester {
		private FluentMappingSource source;

		[SetUp]
		public void Setup() {
			source = new FluentMappingSource("foo");
			source.AddFromAssemblyContaining<CustomerMapping>();
		}

		[Test]
		public void Should_load_mappings() {
			bool mappingWasFound = source.Any(x => x.GetType() == typeof(CustomerMapping));
			Assert.That(mappingWasFound);
		}

		[Test]
		public void Should_build_xml() {
			var document = source.CreateDocument();
			document.ShouldHaveElement("Database").ShouldHaveAttribute("Name", "foo");
		}

		[Test]
		public void AddFromAssembly_should_throw_when_assembly_is_null() {
			Assert.Throws<ArgumentNullException>(() => source.AddFromAssembly(null));
		}

		[Test]
		public void AddModification_should_throw_for_null_argument() {
			Assert.Throws<ArgumentNullException>(() => source.AddModification(null));
		}

		[Test]
		public void Indexer_should_get_mapping_for_type() {
			var mapping = source[typeof(Customer)];
			Assert.IsInstanceOf<CustomerMapping>(mapping);
		}

		[Test]
		public void Should_add_mapping() {
			source = new FluentMappingSource("foo");
			source.AddMapping(new CustomerMapping());

			Assert.That(source.Count(), Is.EqualTo(1));
		}

		[Test]
		public void Should_apply_modifications() {
			source.AddModification(new TestModification());
			source.CreateMappingSource().GetModel(typeof(DataContext)); //CreateMappingSource();

			Assert.That(source.First().CustomProperties["foo"], Is.EqualTo("bar"));
		}

		private class TestModification : IMappingModification {
			public void ApplyTo(IMapping mapping) {
				mapping.CustomProperties.Add("foo", "bar");
			}
		}
	}
}