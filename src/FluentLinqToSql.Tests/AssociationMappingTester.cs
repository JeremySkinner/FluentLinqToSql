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
	using System.Linq;
	using System.Xml.Linq;
	using Mappings;
	using NUnit.Framework;

	[TestFixture]
	public class AssociationMappingTester {
		private IAssociationMapping<Customer, Order> mapping;

		[SetUp]
		public void Setup() {
			mapping = new AssociationMapping<Customer, Order>(typeof(Customer).GetProperty("Orders"));
		}

		[Test]
		public void Should_generate_correct_element_name() {
			Assert.That(MappingXml.Name.LocalName, Is.EqualTo("Association"));
		}

		[Test]
		public void Should_generate_member_attribute() {
			MappingXml.ShouldHaveAttribute("Member", "Orders");
		}

		[Test]
		public void Should_generate_foreign_key_name() {
			mapping.ForeignKeyName("Foo");
			MappingXml.ShouldHaveAttribute("Name", "Foo");
		}

		[Test]
		public void Should_generate_storage() {
			mapping.Storage("_orders");
			MappingXml.ShouldHaveAttribute("Storage", "_orders");
		}

		[Test]
		public void Should_generate_DeleteRule() {
			mapping.DeleteRule("Bar");
			MappingXml.ShouldHaveAttribute("DeleteRule", "Bar");
		}

		[Test]
		public void Should_generate_thiskey() {
			mapping.ThisKey("Foo");
			MappingXml.ShouldHaveAttribute("ThisKey", "Foo");
		}

		[Test]
		public void Should_generate_multiple_ThisKeys() {
			mapping.ThisKey("Foo").ThisKey("Bar");
			MappingXml.ShouldHaveAttribute("ThisKey", "Foo,Bar");
		}

		[Test]
		public void Should_specify_ThisKey_with_lambda() {
			mapping.ThisKey(x => x.Surname);
			MappingXml.ShouldHaveAttribute("ThisKey", "Surname");
		}

		[Test]
		public void Should_generate_otherkey() {
			mapping.OtherKey("Foo");
			MappingXml.ShouldHaveAttribute("OtherKey", "Foo");
		}

		[Test]
		public void Should_generate_multiple_otherkeys() {
			mapping.OtherKey("Foo").OtherKey("Bar");
			MappingXml.ShouldHaveAttribute("OtherKey", "Foo,Bar");
		}

		[Test]
		public void Should_specify_OtherKey_with_lambda() {
			mapping.OtherKey(x => x.Id);
			MappingXml.ShouldHaveAttribute("OtherKey", "Id");
		}

		[Test]
		public void Should_store_custom_property() {
			mapping.CastTo<IPropertyMapping>().CustomProperties["foo"] = "bar";
			Assert.That(mapping.CastTo<IPropertyMapping>().CustomProperties["foo"], Is.EqualTo("bar"));
		}


		private XElement MappingXml {
			get { return mapping.CastTo<IPropertyMapping>().ToXml().Single(); }
		}
	}
}