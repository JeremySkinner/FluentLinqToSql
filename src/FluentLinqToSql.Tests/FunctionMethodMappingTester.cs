namespace FluentLinqToSql.Tests {
	using System;
	using System.Data.Linq;
	using System.Linq;
	using System.Xml.Linq;
	using Mappings;
	using NUnit.Framework;

	[TestFixture]
	public class FunctionMethodMappingTester {
		private IFunctionMethodMapping mapping;
		
		public ISingleResult<Customer> GetCustomersByName(string name) { throw new NotSupportedException(); }

		[SetUp]
		public void Setup() {
			mapping = new FunctionMethodMapping(GetType().GetMethod("GetCustomersByName"), new[] {
             	new FunctionParameterMapping(
					GetType().GetMethod("GetCustomersByName").GetParameters()[0]
				),
            });
		}

		[Test]
		public void Method_should_return_method() {
			mapping.CastTo<FunctionMethodMapping>().Method.ShouldEqual(GetType().GetMethod("GetCustomersByName"));
		}

		[Test]
		public void Should_generate_name() {
			mapping.Attributes["Name"].ShouldEqual("GetCustomersByName");
		}

		[Test]
		public void Custom_name_should_override_built_in_name() {
			mapping.Named("Foo");
			mapping.Attributes["Name"].ShouldEqual("Foo");
		}

		[Test]
		public void Should_have_name_attribute_in_xml() {
			MappingXml.ShouldHaveAttribute("Name", "GetCustomersByName");
		}

		[Test]
		public void Should_have_name_attribute_in_xml_with_custom_name() {
			mapping.Named("Foo");
			MappingXml.ShouldHaveAttribute("Name", "Foo");
		}

		[Test]
		public void Should_have_method_attribute_in_xml() {
			MappingXml.ShouldHaveAttribute("Method", "GetCustomersByName");
		}

		[Test]
		public void Composable_should_set_IsComposable_attribute() {
			mapping.Composable();
			MappingXml.ShouldHaveAttribute("IsComposable", "true");
		}

		[Test]
		public void Should_include_parameters_in_mapping() {
			MappingXml
				.ShouldHaveElement("Parameter")
				.ShouldHaveAttribute("Parameter", "name");
		}

		[Test]
		public void Should_set_element_type() {
			mapping.ElementType<Customer>();
			MappingXml
				.ShouldHaveElement("ElementType")
				.ShouldHaveAttribute("Name", typeof(Customer).FullName);
		}

		[Test]
		public void Should_have_multiple_element_types() {
			mapping.ElementType<Customer>().ElementType<Order>();

			//Two ElementType elements and one parameter
			MappingXml.Elements().Count().ShouldEqual(3);
		}

		[Test]
		public void Should_set_elementtype_with_nested_type_mapping() {
			mapping.ElementType<Customer>(returnType => {
				returnType.Map(x => x.Surname);
			});

			MappingXml.ShouldHaveElement("ElementType")
				.ShouldHaveElement("Column").ShouldHaveAttribute("Name", "Surname");
		}

		private XElement MappingXml {
			get { return mapping.ToXml().Single(); }
		}
	}
}