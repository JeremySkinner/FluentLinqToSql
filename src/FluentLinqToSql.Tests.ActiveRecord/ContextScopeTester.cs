namespace FluentLinqToSql.Tests.ActiveRecord {
	using System;
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.Tests.ActiveRecord.Model;
	using NUnit.Framework;

	[TestFixture]
	public class ContextScopeTester {

		[Test]
		public void ThrowsWhenNoCurrentScope() {
			ActiveRecordConfiguration.Configure(c => {
				c.ConnectionStringIs("foo");
				//c.MapTypes(typeof(Customer));
			});

			typeof(InvalidOperationException).ShouldBeThrownBy(() => { var x = ContextScope.Current; });
		}

		[Test]
		public void SetsCurrentScope() {
			ActiveRecordConfiguration.Configure(c => {
				c.ConnectionStringIs("foo");
				//c.MapTypes(typeof(Customer));
			});

			using(var scope = new ContextScope()) {
				ContextScope.Current.ShouldBeTheSameAs(scope);
			}
		}

		[Test]
		public void CreatesContext() {
			ActiveRecordConfiguration.Configure(c => {
				c.ConnectionStringIs("foo");
				//c.MapTypes(typeof(Customer));
			});

			using (new ContextScope()) {
				ContextScope.Current.Context.ShouldNotBeNull();
			}
		}

		[Test]
		public void UsesAlternativeScopeStorage() {
			ActiveRecordConfiguration.Configure(c => {
				c.ConnectionStringIs("foo");
				//c.MapTypes(typeof(Customer));
				c.ScopeStorage(new TestScopeStorage());
			});

			using(var scope = new ContextScope()) {
				TestScopeStorage.Scope.ShouldBeTheSameAs(scope);
			}
		}

		[Test]
		public void CleansUpScope() {
			ActiveRecordConfiguration.Configure(c => {
				c.ConnectionStringIs("foo");
				//c.MapTypes(typeof(Customer));
				c.ScopeStorage(new TestScopeStorage());
			});

			using (var scope = new ContextScope()) {
				TestScopeStorage.Scope.ShouldBeTheSameAs(scope);
			}

			TestScopeStorage.Scope.ShouldBeNull();
		}

		[SetUp]
		public void Setup() {
			ActiveRecordConfiguration.Reset();
			
		}

		[TearDown]
		public void Teardown() {
			ActiveRecordConfiguration.Reset();
			
		}

		private class TestScopeStorage : IScopeStorage {
			public static ContextScopeBase Scope;

			public void StoreScope(ContextScopeBase scope, Type contextType) {
				Scope = scope;
			}

			public ContextScopeBase GetScope(Type contextType) {
				throw new NotImplementedException();
			}

			public void RemoveScope(Type contextType) {
				Scope = null;
			}
		}
	}
}