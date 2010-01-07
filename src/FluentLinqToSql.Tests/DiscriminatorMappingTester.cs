namespace FluentLinqToSql.Tests {
	using System.Linq;
	using Mappings;
	using NUnit.Framework;

	[TestFixture]
	public class DiscriminatorMappingTester {
		private IDiscriminatorMapping<Customer, int> mapping;
		private IMapping parentMapping; 

		[SetUp]
		public void Setup() {
			parentMapping = new TypeMapping<Customer>();
			mapping = new DiscriminatorMapping<Customer, int>(parentMapping);
		}

		[Test]
		public void Should_output_xml_for_nested_elements() {
			mapping.SubClass<PreferredCustomer>(1, x => {});
			mapping.SubClass<TroublesomeCustomer>(2, x=>{});

			mapping.ToXml().Count().ShouldEqual(2);
			mapping.ToXml().First().ShouldBeNamed("Type").ShouldHaveAttribute("Name", typeof(PreferredCustomer).FullName);
			mapping.ToXml().Last().ShouldBeNamed("Type").ShouldHaveAttribute("Name", typeof(TroublesomeCustomer).FullName);
		}

		[Test]
		public void Subclass_specific_properties_should_be_mapped() {
			mapping.SubClass<PreferredCustomer>(1, preferredCustomer => {
				preferredCustomer.Map(x => x.Discount);
			});

			mapping.ToXml().Single().ShouldHaveElement("Column").ShouldHaveAttribute("Name", "Discount");
		}

		[Test]
		public void Should_set_discriminator_value_on_base_class() {
			mapping.BaseClassDiscriminatorValue(1).ToXml();
			parentMapping.ToXml().Single().ShouldHaveAttribute("InheritanceCode", "1");
		}

		[Test]
		public void When_no_default_is_specified_the_base_class_should_be_the_default() {
			mapping.SubClass<PreferredCustomer>(1, preferredCustomer => {
				preferredCustomer.Map(x => x.Discount);
			});
			mapping.ToXml();
			parentMapping.ToXml().Single().ShouldHaveAttribute("IsInheritanceDefault", "true");
		}

		[Test]
		public void When_default_is_specified_base_class_should_not_be_the_Default() {
			mapping.SubClass<PreferredCustomer>(1, preferredCustomer => {
				preferredCustomer.InheritanceDefault();
				preferredCustomer.Map(x => x.Discount);
			});
			mapping.ToXml().Single().ShouldHaveAttribute("IsInheritanceDefault", "true");
			parentMapping.ToXml().Single().Attribute("IsInheritanceDefault").ShouldBeNull();

		}
	}
}