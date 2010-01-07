namespace FluentLinqToSql.Tests {
	using System;
	using System.Data.Linq;
	using System.Linq;
	using System.Xml.Linq;
	using Mappings;
	using NUnit.Framework;

	[TestFixture]
	public class FunctionParameterMappingTester {
		private IFunctionParameterMapping mapping;

		public ISingleResult<Customer> GetCustomersInCountry(string country) { throw new NotImplementedException(); }

		[SetUp]
		public void Setup() {
			var parameter = GetType().GetMethod("GetCustomersInCountry").GetParameters().Single();
			mapping = new FunctionParameterMapping(parameter);
		}

		[Test]
		public void Should_create_parameter_element() {
			MappingXml.ShouldBeNamed("Parameter");
		}

		[Test]
		public void Should_have_parameter_attribute() {
			MappingXml.ShouldHaveAttribute("Parameter", "country");
		}

		[Test]
		public void Should_have_DbType_attribute() {
			mapping.DbType("nvarchar(250)");
			MappingXml.ShouldHaveAttribute("DbType", "nvarchar(250)");
		}

		[Test]
		public void Should_have_name_attribute() {
			mapping.Named("foo");
			MappingXml.ShouldHaveAttribute("Name", "foo");
		}

		private XElement MappingXml {
			get { return mapping.ToXml().Single(); }
		}
	}
}