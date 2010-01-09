namespace FluentLinqToSql.Tests.ActiveRecord {
	using System.Linq;
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.Tests.ActiveRecord.Model;
	using NUnit.Framework;

	public class FindAllTester : IntegrationTest {
		[Test]
		public void FindsAllCustomers() {
			using(new ContextScope()) {
				var cmd1 = CreateCommand();
				cmd1.CommandText = "insert into Customer (Name) values ('foo');";
				cmd1.ExecuteNonQuery();

				var cmd2 = CreateCommand();
				cmd2.CommandText = "insert into Customer (Name) values ('foo');";
				cmd2.ExecuteNonQuery();

				var cmd3 = CreateCommand();
				cmd3.CommandText = "insert into Customer (Name) values ('foo');";
				cmd3.ExecuteNonQuery();

				var customers = Customer.FindAll().ToList();
				customers.Count.ShouldEqual(3);
			}
		}
	}
}