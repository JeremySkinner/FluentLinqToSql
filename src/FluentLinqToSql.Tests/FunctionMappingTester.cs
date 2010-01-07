namespace FluentLinqToSql.Tests {
	using System;
	using System.Data.Linq;
	using System.Linq;
	using Mappings;
	using NUnit.Framework;

	[TestFixture]
	public class FunctionMappingTester {
		private FunctionMapping<FunctionMappingTester> mapping;

		public ISingleResult<Customer> GetCustomers() { throw new NotImplementedException(); }
		public ISingleResult<Customer> GetCustomersByName(string name) { throw new NotImplementedException(); }


		[SetUp]
		public void Setup() {
			mapping = new FunctionMapping<FunctionMappingTester>();
		}

		[Test]
		public void Should_throw_when_null_is_passed_to_Map() {
			 Assert.Throws<ArgumentNullException>(() => mapping.Map(null));
		}

		[Test]
		public void Map_should_return_instance_of_IFunctionMethodMapping() {
			mapping.Map(x => x.GetCustomers()).ShouldBeOfType<FunctionMethodMapping>();
		}

		[Test]
		public void Map_should_locate_method() {
			mapping.Map(x => x.GetCustomers()).CastTo<FunctionMethodMapping>().Method.ShouldEqual(GetType().GetMethod("GetCustomers"));
		}

		[Test]
		public void PropertyMappings_should_return_one_instance_per_mapped_method() {
			mapping.Map(x => x.GetCustomers());
			mapping.Map(x => x.GetCustomersByName("foo"));

			mapping.Mappings.Count.ShouldEqual(2);
		}

		[Test]
		public void ToXml_should_produce_one_xelement_per_mapped_method() {
			mapping.Map(x => x.GetCustomers());
			mapping.Map(x => x.GetCustomersByName("foo"));
			mapping.ToXml().Count().ShouldEqual(2);
		}

		[Test]
		public void Should_store_custom_property() {
			mapping.CustomProperties["foo"] = "bar";
			mapping.CustomProperties["foo"].ShouldEqual("bar");
		}

		[Test]
		public void MappedType_should_return_type_being_mapped() {
			mapping.MappedType.ShouldEqual(typeof(FunctionMappingTester));
		}

		[Test]
		public void Should_map_simple_parameter() {
			mapping.Map(x => x.GetCustomersByName(null));
			mapping.ToXml().Single()
				.ShouldHaveElement("Parameter")
				.ShouldHaveAttribute("Parameter", "name");
		}

		[Test]
		public void Should_map_complex_parameter() {
			mapping.Map(x => x.GetCustomersByName(
				FunctionMapping<object>.Parameter<string>(
					param => param.Named("Foo").DbType("Bar")
				)
			));

			mapping.ToXml().Single()
				.ShouldHaveElement("Parameter")
				.ShouldHaveAttribute("Parameter", "name")
				.ShouldHaveAttribute("Name", "Foo")
				.ShouldHaveAttribute("DbType", "Bar");
		}

	}
}