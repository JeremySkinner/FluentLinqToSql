namespace FluentLinqToSql.Tests.ActiveRecord {
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.Tests.ActiveRecord.Model;
	using NUnit.Framework;

	public class DeleteTester : IntegrationTest {
		[Test]
		public void EntityIsDeleted() {
			Customer customer;

			using (new ContextScope()) {
				customer = new Customer { Name = "Jeremy" };
				customer.Save();
			}

			using (new ContextScope()) {
				ContextScope.Current.Context.GetTable<Customer>().Attach(customer);

				customer.Delete();
			}

			using (new ContextScope()) {
				customer = Customer.FindById(customer.Id);
				customer.ShouldBeNull();
			}
		}
	}
}