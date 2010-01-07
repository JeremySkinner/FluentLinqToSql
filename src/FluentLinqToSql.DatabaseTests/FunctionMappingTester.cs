namespace FluentLinqToSql.DatabaseTests {
	using Entities;
	using NUnit.Framework;
	using System.Linq;
	[TestFixture]
	public class FunctionMappingTester : DatabaseTest {

		protected override void Setup() {
			Execute("IF EXISTS (SELECT 1 from sys.objects where Object_id = OBJECT_ID('CustomersByName')) DROP FUNCTION dbo.CustomersByName");
			Execute(@"CREATE FUNCTION dbo.CustomersByName(@name nvarchar(250)) RETURNS Table
AS
RETURN (SELECT * FROM Customers WHERE Name = @name)
");
		}

		[Test]
		public void Should_execute_function() {
			using (var context = DataContextFactory()) {
				context.GetTable<Customer>().InsertOnSubmit(new Customer() { Name = "Jeremy" });
				context.GetTable<Customer>().InsertOnSubmit(new Customer() { Name = "Mark" });
				context.GetTable<Customer>().InsertOnSubmit(new Customer() { Name = "Jeremy" });
				context.SubmitChanges();
			}

			using(var context = (TestDataContext)DataContextFactory()) {
				var customers = context.CustomersByName("Jeremy");
				Assert.That(customers.Count(), Is.EqualTo(2));
			}
		}
	}
}