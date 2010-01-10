namespace FluentLinqToSql.Tests.ActiveRecord {
	using System.Collections.Specialized;
	using FluentLinqToSql.Tests.ActiveRecord.Model;
	using NUnit.Framework;

	[TestFixture]
	public class UpdateFromTester {
		[Test]
		public void UpdatesProperties() {
			var form = new NameValueCollection {
				{ "Id", "1" },
				{ "Name", "Jeremy" }
			};

			var customer = new Customer().UpdateFrom(form);

			customer.Id.ShouldEqual(1);
			customer.Name.ShouldEqual("Jeremy");
		}

		[Test]
		public void UpdatesPropertiesWithPrefix() {
			var form = new NameValueCollection {
				{ "cust.Id", "1" },
				{ "cust.Name", "Jeremy" }
			};

			var customer = new Customer().UpdateFrom(form, "cust");

			customer.Id.ShouldEqual(1);
			customer.Name.ShouldEqual("Jeremy");
		}
	}
}