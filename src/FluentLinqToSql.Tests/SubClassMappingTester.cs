namespace FluentLinqToSql.Tests {
	using System.Linq;
	using Mappings;
	using NUnit.Framework;

	[TestFixture]
	public class SubClassMappingTester {
		private SubClassMapping<Customer, int> mapping;

		[SetUp]
		public void Setup() {
			mapping = new SubClassMapping<Customer, int>(3);
		}

		[Test]
		public void InheritanceDefault_should_set_InheritanceDefault_to_true() {
			mapping.InheritanceDefault();
			mapping.ToXml().Single().ShouldHaveAttribute("IsInheritanceDefault", "true");
		}

		[Test]
		public void Should_output_inheritance_code() {
			mapping.ToXml().Single().ShouldHaveAttribute("InheritanceCode", "3");
		}
	}
}