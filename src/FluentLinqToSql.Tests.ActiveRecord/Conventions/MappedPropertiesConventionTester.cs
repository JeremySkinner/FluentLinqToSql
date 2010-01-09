namespace FluentLinqToSql.Tests.ActiveRecord.Conventions {
	using System.Collections.Generic;
	using System.Data.Linq.Mapping;
	using System.Linq;
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.ActiveRecord.Conventions;
	using FluentLinqToSql.Internal;
	using NUnit.Framework;

	[TestFixture]
	public class MappedPropertiesConventionTester {
		MappedPropertiesConvention convention;

		[SetUp]
		public void Setup() {
			convention = new MappedPropertiesConvention();
			
		}

		[Test]
		public void ConvertsPropertyToColumn() {
			var mappings = convention.CreateColumnMappings(typeof(Foo), GetMeta<Foo>());
			mappings.Count().ShouldEqual(1);
		}

		[Test]
		public void CopiesMetadataToMapping() {
			var mappings = convention.CreateColumnMappings(typeof(Foo), GetMeta<Foo>());
			var mapping = mappings.Single();

			mapping.Attributes[Constants.AutoSync].ShouldEqual("Never");
			mapping.Attributes[Constants.Name].ShouldEqual("x");
			mapping.Attributes[Constants.CanBeNull].ShouldEqual("false");
			mapping.Attributes[Constants.DbType].ShouldEqual("y");
			mapping.Attributes[Constants.Expression].ShouldEqual("z");
			mapping.Attributes[Constants.IsDbGenerated].ShouldEqual("true");
			//mapping.Attributes[Constants.IsDiscriminator].ShouldEqual("true"); TODO Discriminator
			mapping.Attributes[Constants.IsVersion].ShouldEqual("true");
			mapping.Attributes[Constants.IsVersion].ShouldEqual("true");
			mapping.Attributes[Constants.Storage].ShouldEqual("id");
			mapping.Attributes[Constants.UpdateCheck].ShouldEqual("Never");
		}

		private IEnumerable<ColumnMetaData> GetMeta<T>() {
			return new MetaDataBuilderConvention().GetMetaData(typeof(T)).OfType<ColumnMetaData>();
		}

		private class Foo {
			[Column(IsPrimaryKey = true,
				AutoSync = AutoSync.Never,
				Name = "x",
				CanBeNull = false,
				DbType = "y",
				Expression = "z",
				IsDbGenerated = true,
				IsDiscriminator = true,
				IsVersion = true,
				Storage = "id",
				UpdateCheck = UpdateCheck.Never)]
			public int Id { get; set; }
		}
	}
}