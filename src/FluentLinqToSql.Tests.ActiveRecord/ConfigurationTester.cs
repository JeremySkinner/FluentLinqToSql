namespace FluentLinqToSql.Tests.ActiveRecord {
	using System;
	using System.Data.Linq.Mapping;
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.Tests.ActiveRecord.Model;
	using NUnit.Framework;

	[TestFixture]
	public class ConfigurationTester {

		[SetUp]
		public void Setup() {
			ActiveRecordConfiguration.Reset();
			
		}

		[Test]
		public void SetsConnectionString() {
			ActiveRecordConfiguration.Configure(x => {
				x.ConnectionStringIs("foo");
//				x.MapTypes(typeof(Customer));
			});

			ActiveRecordConfiguration.Current.ConnectionString.ShouldEqual("foo");
		}

		[Test]
		public void SetsConnectionStringFromConfig() {
			ActiveRecordConfiguration.Configure(x => {
				x.ConnectionStringFromConfigFile("foo");
//				x.MapTypes(typeof(Customer));
			});

			ActiveRecordConfiguration.Current.ConnectionString.ShouldEqual("bar");
		}

		[Test]
		public void SetsCustomScopeStorage() {
			ActiveRecordConfiguration.Configure(x => {
				x.ConnectionStringIs("foo");
//				x.MapTypes(typeof(Customer));
				x.ScopeStorage(new TestScopeStorage());
			});

			ActiveRecordConfiguration.Current.ScopeStorage.ShouldBe<TestScopeStorage>();
		}

		[Test]
		public void SetsContextFactory() {
			DataContextFactory contextFactory = (connectionString, source) => null;
			ActiveRecordConfiguration.Configure(x => {
				x.ConnectionStringIs("foo");
//				x.MapTypes(typeof(Customer));
				x.DataContextFactory(contextFactory);
			});

			ActiveRecordConfiguration.Current.DataContextFactory.ShouldBeTheSameAs(contextFactory);
		}

	
		[Test]
		public void SetsMappingSource() {
			ActiveRecordConfiguration.Configure(x => {
				x.ConnectionStringIs("foo");
//				x.MapTypes(typeof(Customer));
				x.MappingSource(new AttributeMappingSource());
			});

			ActiveRecordConfiguration.Current.MappingSource.ShouldBe<AttributeMappingSource>();

		}

		[Test]
		public void BuildsConnectionStringUsingServerName() {
			ActiveRecordConfiguration.Configure(x => {
				x.ConnectToSqlServer("server", "database");
//				x.MapTypes(typeof(Customer));
			});

			ActiveRecordConfiguration.Current.ConnectionString.ShouldEqual("Data Source=server;Initial Catalog=database;Integrated Security=SSPI;");

		}

		[Test]
		public void BuildsConnectionStringUsingServerNameUserAndPassword() {
			ActiveRecordConfiguration.Configure(x => {
				x.ConnectToSqlServer("server", "database", "user", "password");
//				x.MapTypes(typeof(Customer));
			});

			ActiveRecordConfiguration.Current.ConnectionString.ShouldEqual("Data Source=server;Initial Catalog=database;User ID=user;Password=password;");
		}

		public class TestScopeStorage : IScopeStorage {
			public void StoreScope(ContextScopeBase scope, Type contextType) {
				throw new NotImplementedException();
			}

			public ContextScopeBase GetScope(Type contextType) {
				throw new NotImplementedException();
			}

			public void RemoveScope(Type contextType) {
				throw new NotImplementedException();
			}
		}

		[TearDown]
		public void Teardown() {
			ActiveRecordConfiguration.Reset();
			
		}

	}
}