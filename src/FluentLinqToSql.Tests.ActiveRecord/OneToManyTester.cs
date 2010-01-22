namespace FluentLinqToSql.Tests.ActiveRecord {
	using System;
	using System.Data.Linq;
	using System.Linq;
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.Tests.ActiveRecord.Model;
	using NUnit.Framework;

	public class OneToManyTester  : IntegrationTest {

		[Test]
		public void MapsOneToManyUsingConventions() {
			var cust = new Customer() { Name = "Jeremy" };
			var order = new Order() { Customer = cust };

			using(ContextScope.Begin()) {
				cust.Orders.Add(order);
				cust.Save();
			}

			using (ContextScope.Begin()) {
				cust = Customer.FindAll().Single();
				cust.Orders.Count().ShouldEqual(1);
			}
		}
	}
}