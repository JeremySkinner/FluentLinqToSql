namespace FluentLinqToSql.DatabaseTests {
	using Entities;
	using NUnit.Framework;

	[TestFixture]
	public class OneToOneMappingTester : DatabaseTest {
		[Test]
		public void Should_map_one_to_one() {
			using(var test = Verify<Customer>()) {
				test.TestProperty(x => x.ContactDetails, new CustomerContact() { Email = "Foo", PhoneNumber = "Bar" });
			}
		}
	}
}