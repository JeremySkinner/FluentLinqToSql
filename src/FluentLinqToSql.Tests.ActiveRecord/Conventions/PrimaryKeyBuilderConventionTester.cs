namespace FluentLinqToSql.Tests.ActiveRecord.Conventions {
	using System.Collections.Generic;
	using System.Data.Linq.Mapping;
	using System.Linq;
	using System.Reflection;
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.Internal;
	using FluentLinqToSql.ActiveRecord.Conventions;
	using NUnit.Framework;

	[TestFixture]
	public class PrimaryKeyBuilderConventionTester {
		PrimaryKeyBuilderConvention convention;

		[SetUp]
		public void Setup() {
			convention = new PrimaryKeyBuilderConvention();
			
		}

		[Test]
		public void GetsIdProperty() {
			var members = convention.CreateColumnMappings(typeof(Foo1), GetMeta<Foo1>());
			members.Single().Property.Name.ShouldEqual("Id");
		}

		[Test]
		public void GetsCustomPrimaryKeys() {
			var members = convention.CreateColumnMappings(typeof(Foo2), GetMeta<Foo2>());
			members.Count().ShouldEqual(2);
			members.First().Property.Name.ShouldEqual("Field1");
			members.Last().Property.Name.ShouldEqual("Field2");                
		}

		private IEnumerable<ColumnMetaData> GetMeta<T>() {
			return new MetaDataBuilderConvention().GetMetaData(typeof (T)).OfType<ColumnMetaData>();
		}

		private class Foo1 {
			public int Id { get; set; }
		}

		private class Foo2 {
			[Column(IsPrimaryKey = true)]
			public int Field1 { get; set; }

			[Column(IsPrimaryKey = true)]
			public string Field2 { get; set; }
		}

		
	}
}