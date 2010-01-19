namespace FluentLinqToSql.Tests.ActiveRecord {
	using System.Data.Linq;
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.Tests.ActiveRecord.Model;
	using NUnit.Framework;

	public class ManyToOneTester : IntegrationTest {
		[Test]
		public void MapsManyToOneUsingConventions() {
			var cust = new Customer() { Name = "Jeremy" };
			var order = new Order() { Customer = cust };

			using(ContextScope.Begin()) {
				cust.Save();
				order.Save();	
			}

			using(ContextScope.Begin()) {
				order = Order.FindById(order.Id);
				order.Customer.Name.ShouldEqual("Jeremy");
			}
		}
	}
}