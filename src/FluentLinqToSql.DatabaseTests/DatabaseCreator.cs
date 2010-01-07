namespace FluentLinqToSql.DatabaseTests {
	using NUnit.Framework;

	[TestFixture, Explicit, Category("CreateDatabase")]
	public class DatabaseCreator {
		
		//Run this test to create the test database. 
		//You'll need a default instance of SQL Server running locally. 
		//The connection string is specified in TestDataContext.cs
		[Test]
		public void Recreate_Database() {

			var context = new TestDataContext(new TestMappingSource());
			
			if(context.DatabaseExists()) {
				context.DeleteDatabase();
			}

			context.CreateDatabase();
		}
	}
}