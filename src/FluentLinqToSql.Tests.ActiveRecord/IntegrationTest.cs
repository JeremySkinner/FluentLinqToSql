namespace FluentLinqToSql.Tests.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data.SqlServerCe;
	using System.IO;
	using System.Linq;
	using FluentLinqToSql;
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.Tests.ActiveRecord.Model;
	using NUnit.Framework;

	[TestFixture]
	public abstract class IntegrationTest {
		private static IEnumerable<Type> entityTypes;
		static bool initialized;
		static MappingSource mapping;
		DbConnection connection;

		[TestFixtureSetUp]
		public void BaseSetup() {
			const string conn = "Data Source=Test.sdf;Persist Security Info=False;";

			if (!initialized) {
				initialized = true;
				if (File.Exists("Test.sdf")) {
					File.Delete("Test.sdf");
				}

				using (var engine = new SqlCeEngine(conn)) {
					engine.CreateDatabase();
				}

				var script = File.ReadAllText("Schema.sql").Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

				using (var c = new SqlCeConnection(conn)) {
					c.Open();
					foreach (var line in script) {
						var cmd = c.CreateCommand();
						cmd.CommandText = line;
						cmd.ExecuteNonQuery();
					}
					c.Close();
				}

				/*	mapping = new FluentMappingSource("Test")
						.AddMapping(new CustomerMap());*/
			}

			connection = new SqlCeConnection(conn);
			connection.Open();

			ActiveRecordConfiguration.Configure(cfg => {
				cfg.ConnectionStringIs(conn);
				MapTypes(cfg);
//				cfg.MapTypesFromAssemblyContaining<Customer>();
				cfg.DataContextFactory((c, m) => {
					return new DataContext(connection, m) { Log = Console.Out };
				});
			});
//			Setup();
		}

		protected virtual void MapTypes(IActiveRecordConfiguration cfg) {
			cfg.MapTypesFromAssemblyContaining<Customer>();
		}

		public virtual void Setup() { }

		[TestFixtureTearDown]
		public void BaseTearDown() {
			ActiveRecordConfiguration.Reset();

			var deleteCmd = connection.CreateCommand();
			deleteCmd.CommandText = "delete from Customer";
			deleteCmd.ExecuteNonQuery();
			deleteCmd.CommandText = "delete from Customer2";
			deleteCmd.ExecuteNonQuery();

			connection.Close();
			connection.Dispose();
		}

		public virtual void Teardown() { }

		protected IDbCommand CreateCommand() {
			return connection.CreateCommand();
		}
	}

	/*public class CustomerMap : Mapping<Customer> {
		public CustomerMap() {
			Identity(x => x.Id);
			Map(x => x.Name);
		}
	}*/
}