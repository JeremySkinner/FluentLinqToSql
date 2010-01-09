namespace FluentLinqToSql.Tests.ActiveRecord.Conventions {
	using System.Data.Linq.Mapping;
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.ActiveRecord.Conventions;
	using NUnit.Framework;
	using System.Linq;

	[TestFixture]
	public class MetaDataBuilderConventionTester {
		MetaDataBuilderConvention convention;

		[SetUp]
		public void Setup() {
			convention = new MetaDataBuilderConvention();
			
		}

		[Test]
		public void GetsMetadataForProperty() {
			var metas = convention.GetMetaData(typeof(Foo1));
			var id = metas.SingleOrDefault(x => x.Member.Name == "Id");
			id.ShouldNotBeNull();

		}

		[Test]
		public void GetsMetadataForPropertyWithAttribute() {
			var metas = convention.GetMetaData(typeof(Foo1));
			var name = (ColumnMetaData)metas.Single(x => x.Member.Name == "Name");
			name.Attribute.Name.ShouldEqual("Foo");
		}

		[Test, Ignore("not implemented")]
		public void GetsMetadataForAssociation() {
			
		}

		[Test, Ignore("Not implemented")]
		public void GetsMetadataForAssociationWithAttribute() {
			
		}

		[Test]
		public void IgnoredPropertyShouldBeSkipped() {
			var metas = convention.GetMetaData(typeof(Foo3));
			metas.Count().ShouldEqual(0);
		}

		private class Foo1 {
			public int Id { get; set; }
			[Column(Name = "Foo")]
			public string Name { get; set; }
		}

		private class Foo2 {
			
		}

		private class Foo3 {
			[NotMapped]
			public string Name { get; set; }
		}
	}
}