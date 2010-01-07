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
	using System.Linq;
	using Internal;
	using NUnit.Framework;
	using System.Collections.Generic;

	[TestFixture]
	public class MappingTester {
		private Mapping<Customer> mapping;

		[SetUp]
		public void Setup() {
			mapping = new Mapping<Customer>();
		}

		[Test]
		public void When_ToXml_is_called_a_table_element_should_be_created() {
			mapping.ToXml().Single().ShouldBeNamed("Table");
		}

		[Test]
		public void Table_element_should_have_table_name() {
			mapping.ToXml().Single().ShouldHaveAttribute("Name", "Customer");
		}

		[Test]
		public void Table_element_should_have_custom_table_name() {
			mapping.Named("Foo");
			mapping.ToXml().Single().ShouldHaveAttribute("Name", "Foo");
		}


		[Test]
		public void Table_element_should_have_type_element() {
			mapping.ToXml().Single().ShouldHaveElement("Type");
		}

		[Test]
		public void Type_name_should_be_unaffected_by_custom_table_name() {
			mapping.Named("Foo");
			mapping.ToXml().Single().ShouldHaveElement("Type").ShouldHaveAttribute("Name", typeof(Customer).FullName);
		}

		[Test]
		public void Should_throw_with_invalid_table_name() {
			Assert.Throws<ArgumentNullException>(() => mapping.Named(null));
		}
	}
}