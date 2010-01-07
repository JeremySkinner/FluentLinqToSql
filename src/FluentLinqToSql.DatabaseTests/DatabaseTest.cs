namespace FluentLinqToSql.DatabaseTests {
	using System;
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using NUnit.Framework;
	using TestHelper;

	public abstract class DatabaseTest {
		protected Func<DataContext> DataContextFactory;

		protected virtual void CleanupDatabase() {
			Execute("delete from CustomerContacts; delete from Orders; delete from Customers");
		}
		protected virtual void Setup() {}

		[SetUp]
		public void BaseSetup() {
			DataContextFactory = () => new TestDataContext(CreateMappingSource());
			Setup();
			CleanupDatabase();
		}

		protected MappingTester<T> Verify<T>() where T : class, new() {
			return new MappingTester<T>(DataContextFactory);
		}

		protected void Execute(string sql) {
			using(var ctx = DataContextFactory()) {
				ctx.ExecuteCommand(sql);
			}
		}

		protected virtual MappingSource CreateMappingSource() {
			return new TestMappingSource();
		}
	}
}