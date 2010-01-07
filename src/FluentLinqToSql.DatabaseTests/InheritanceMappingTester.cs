namespace FluentLinqToSql.DatabaseTests {
	using System.Linq;
	using Entities;
	using NUnit.Framework;

	[TestFixture]
	public class InheritanceMappingTester : DatabaseTest {
		[Test]
		public void Should_map_subclass() {
			using(var test = Verify<PreferredCustomer>()) {
				test.TestProperty(x => x.Name, "Jeremy");
				test.TestProperty(x => x.Discount, 5.5);
			}
		}
	}
}