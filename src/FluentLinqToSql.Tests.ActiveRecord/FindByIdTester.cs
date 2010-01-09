namespace FluentLinqToSql.Tests.ActiveRecord
{
	using System;
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.Tests.ActiveRecord.Model;
	using NUnit.Framework;

	public class FindByIdTester : IntegrationTest
	{
		[Test]
		public void FindsCustomerById()
		{
			using (var scope = new ContextScope()) {

				var cmd = CreateCommand();
				cmd.CommandText = "insert into Customer (Name) values ('Jeremy')";
				cmd.ExecuteNonQuery();

				var cmd2 = CreateCommand();
				cmd2.CommandText = "select @@identity";
				var id = Convert.ToInt32(cmd2.ExecuteScalar());

				var cust = Customer.FindById(id);
				cust.ShouldNotBeNull();
			}
		}
	}
}