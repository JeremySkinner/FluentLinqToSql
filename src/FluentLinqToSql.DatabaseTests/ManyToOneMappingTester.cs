namespace FluentLinqToSql.DatabaseTests {
	using System;
	using Entities;
	using NUnit.Framework;

	[TestFixture]
	public class ManyToOneMappingTester : DatabaseTest {
		[Test]
		public void Should_map_many_to_one_association() {
			Customer customer; 
			using(var ctx = DataContextFactory()) {
				customer = new Customer();
				ctx.GetTable<Customer>().InsertOnSubmit(customer);
				ctx.SubmitChanges();
			}

			using(var test = Verify<Order>()) {
				test.TestProperty(x => x.OrderDate, new DateTime(2008, 12, 23));
				test.TestProperty(x => x.Customer, customer);
			}
		}
	}
}