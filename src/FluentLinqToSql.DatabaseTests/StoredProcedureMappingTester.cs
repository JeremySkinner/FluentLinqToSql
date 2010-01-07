namespace FluentLinqToSql.DatabaseTests {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Entities;
	using NUnit.Framework;

	[TestFixture]
	public class StoredProcedureMappingTester : DatabaseTest {

		protected override void Setup() {
			Execute(
				"IF EXISTS (SELECT 1 from sys.objects where Object_id = OBJECT_ID('CustomersAndOrders')) DROP PROC dbo.CustomersAndOrders; "
				+ "IF EXISTS (SELECT 1 from sys.objects where Object_id = OBJECT_ID('CountCustomerOrders')) DROP PROC dbo.CountCustomerOrders; "
				
			);
			Execute(@"CREATE PROC dbo.CustomersAndOrders AS
SELECT * FROM Customers
SELECT * FROM Orders
");
			Execute(@"CREATE PROC dbo.CountCustomerOrders AS
SELECT c.Name AS CustomerName, COUNT(o.Id) AS NumberOfOrders FROM Customers c 
LEFT OUTER JOIN Orders o ON c.Id = o.CustomerId
GROUP BY c.Id, c.Name
");
		}

		[Test]
		public void Should_map_multiple_result_sets() {
			using (var context = (TestDataContext)DataContextFactory()) {
				context.GetTable<Customer>().InsertOnSubmit(new Customer() { Name = "Jeremy" });
				context.GetTable<Customer>().InsertOnSubmit(new Customer() { Name = "Mark", Orders = new List<Order>() { new Order { OrderDate = DateTime.Now } } });
				context.SubmitChanges();
			}

			using (var context = (TestDataContext)DataContextFactory()) {
				var results = context.CustomersAndOrders();
				var customers = results.GetResult<Customer>();
				var orders = results.GetResult<Order>();

				Assert.That(customers.Count(), Is.EqualTo(2));
				Assert.That(orders.Count(), Is.EqualTo(1));
			}
		}

		[Test]
		public void Should_map_custom_results() {
			using (var context = (TestDataContext)DataContextFactory()) {
				context.GetTable<Customer>().InsertOnSubmit(new Customer() { Name = "Jeremy" });
				context.GetTable<Customer>().InsertOnSubmit(new Customer() { Name = "Mark", Orders = new List<Order>() { new Order { OrderDate = DateTime.Now } } });
				context.SubmitChanges();
			}

			using (var context = (TestDataContext)DataContextFactory()) {
				var customersWithOrders = context.CountCustomerOrders().ToList();
				Assert.That(customersWithOrders.Count(), Is.EqualTo(2));

				Assert.That(customersWithOrders.First().NumberOfOrders, Is.EqualTo(0));
				Assert.That(customersWithOrders.First().Name, Is.EqualTo("Jeremy"));

				Assert.That(customersWithOrders.Last().NumberOfOrders, Is.EqualTo(1));
				Assert.That(customersWithOrders.Last().Name, Is.EqualTo("Mark"));
			}
		}
	}
}