namespace FluentLinqToSql.Tests.ActiveRecord {
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.Tests.ActiveRecord.Model;
	using NUnit.Framework;

	public class ConventionOverrideTester : IntegrationTest {
		[Test]
		public void OverridesTableName() {
			var cust = new CustomisedCustomer();

			using (new ContextScope()) {
				cust.Save();
			}

			using(new ContextScope()) {
				cust = CustomisedCustomer.FindById(cust.CustomerId);
				cust.ShouldNotBeNull();
			}
		}

		[Test]
		public void OveridesColumnName() {
			var cust = new CustomisedCustomer() { Name = "Jeremy" };

			using (new ContextScope()) {
				cust.Save();
			}

			using (new ContextScope()) {
				cust = CustomisedCustomer.FindById(cust.CustomerId);
				cust.Name.ShouldEqual("Jeremy");
			}
		}

		[Test]
		public void IgnoresProperty() {
			var cust = new CustomisedCustomer() { NotMapped = "foo" };

			using (new ContextScope()) {
				cust.Save();
			}

			using (new ContextScope()) {
				cust = CustomisedCustomer.FindById(cust.CustomerId);
				cust.NotMapped.ShouldBeNull();
			}
		}

		/*protected override void MapTypes(IActiveRecordConfiguration cfg) {
			cfg.MapTypes(typeof(CustomisedCustomer));
		}*/
	}
}