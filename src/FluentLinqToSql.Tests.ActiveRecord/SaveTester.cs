namespace FluentLinqToSql.Tests.ActiveRecord {
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.Tests.ActiveRecord.Model;
	using NUnit.Framework;

	public class SaveTester : IntegrationTest {
		[Test]
		public void SavesCustomer() {
			Customer customer;

			using(new ContextScope()) {
				customer = new Customer {
					Name = "Jeremy"
				};

				customer.Save();
			}

			using (new ContextScope()) {
				var customer2 = Customer.FindById(customer.Id);
				customer2.Name.ShouldEqual("Jeremy");
				customer.ShouldNotBeTheSameAs(customer2);
			}
		}

		[Test]
		public void DoesNotInsertIfCommitVetoed() {
			Customer customer;

			using (new ContextScope()) {
				ContextScope.Current.VetoCommit();

				customer = new Customer {
					Name = "Jeremy"
				};

				customer.Save();
			}

			using (new ContextScope()) {
				var customer2 = Customer.FindById(customer.Id);
				customer2.ShouldBeNull();
			}
		}
	}
}