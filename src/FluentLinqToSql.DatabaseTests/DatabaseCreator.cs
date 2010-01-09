namespace FluentLinqToSql.DatabaseTests {
	using NUnit.Framework;

	[TestFixture, Explicit, Category("CreateDatabase")]
	public class DatabaseCreator {
		[Test, Explicit, Category("CreateDatabase")]
		public void CreateDb() {
			using (var ctx = new TestDataContext(new TestMappingSource())) {
				if(ctx.DatabaseExists()) {
					ctx.DeleteDatabase();
				}
				ctx.CreateDatabase();
			}
		}
	}
}