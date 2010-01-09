namespace FluentLinqToSql.Tests.ActiveRecord.Conventions {
	using System.Data.Linq.Mapping;
	using FluentLinqToSql.ActiveRecord.Conventions;
	using NUnit.Framework;

	[TestFixture]
	public class TableNameConventionTester {
		TableNameConvention convention;

		[SetUp]
		public void Setup() {
			convention = new TableNameConvention();
			
		}

		[Test]
		public void UsesDefaultTableName() {
			convention.GetTableName(typeof(Foo1)).ShouldEqual("Foo1");
		}

		[Test]
		public void OverridesTableName() {
			convention.GetTableName(typeof(Foo2)).ShouldEqual("Foo3");
		}

		private class Foo1 {
			
		}

		[Table(Name = "Foo3")]
		private class Foo2 {
			
		}
	}
}