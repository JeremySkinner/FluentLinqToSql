namespace FluentLinqToSql.Tests {
	using System;
	using System.Linq;
	using Mappings;
	using NUnit.Framework;
	using System.Collections.Generic;

	[TestFixture]
	public class TypeMappingTester {
		private TypeMapping<Customer> mapping;

		[SetUp]
		public void Setup() {
			mapping = new TypeMapping<Customer>();
		}

		[Test]
		public void When_ToXml_is_called_a_Type_element_should_be_created() {
			mapping.ToXml().Single().ShouldBeNamed("Type");
		}

		[Test]
		public void Type_element_should_have_type_name() {
			mapping.ToXml().Single().ShouldHaveAttribute("Name", typeof(Customer).FullName);
		}

		[Test]
		public void Should_throw_when_expression_is_null() {
			Assert.Throws<ArgumentNullException>(() => mapping.Map<object>(null));
		}

		[Test]
		public void Should_create_instance_of_ColumnMapping() {
			Assert.IsInstanceOf<ColumnMapping>(mapping.Map(c => c.Id));
		}

		[Test]
		public void Map_should_correctly_locate_property() {
			var columnMapping = (ColumnMapping)mapping.Map(x => x.Id);
			Assert.That(columnMapping.Property, Is.EqualTo(typeof(Customer).GetProperty("Id")));
		}

		[Test]
		public void Should_throw_when_a_non_memberexpression_passed_to_Map() {
			Assert.Throws<ArgumentException>(() => mapping.Map(x => x.ToString()));
		}

		[Test]
		public void Identity_should_generate_Identity_mapping() {
			var col = (IPropertyMapping)mapping.Identity(c => c.Id);
			Assert.That(col.Attributes["IsPrimaryKey"], Is.EqualTo("true"));
			Assert.That(col.Attributes["IsDbGenerated"], Is.EqualTo("true"));
		}

		[Test]
		public void Should_throw_when_HasMany_expression_is_null() {
			Assert.Throws<ArgumentNullException>(() => mapping.HasMany<Order>(null));
		}

		[Test]
		public void Should_create_instance_of_HasManyMapping() {
			Assert.IsInstanceOf<HasManyMapping<Customer, Order>>(mapping.HasMany(c => c.Orders));
		}

		[Test]
		public void HasMany_should_locate_property() {
			var hasMany = (HasManyMapping<Customer, Order>)mapping.HasMany(c => c.Orders);
			Assert.That(hasMany.Property, Is.EqualTo(typeof(Customer).GetProperty("Orders")));
		}

		[Test]
		public void Should_throw_when_a_non_member_expression_passed_to_HasMany() {
			Assert.Throws<ArgumentException>(() => mapping.HasMany(x => new List<string>()));
		}

		[Test]
		public void Should_create_instance_of_HasOneMapping() {
			Assert.IsInstanceOf<HasOneMapping<Customer, Company>>(mapping.HasOne(c => c.Company));
		}

		[Test]
		public void HasOne_should_locate_property() {
			var hasMany = (HasOneMapping<Customer, Company>)mapping.HasOne(c => c.Company);
			Assert.That(hasMany.Property, Is.EqualTo(typeof(Customer).GetProperty("Company")));
		}

		[Test]
		public void Should_throw_when_a_non_member_expression_passed_to_HasOne() {
			Assert.Throws<ArgumentException>(() => mapping.HasOne(x => "foo"));
		}

		[Test]
		public void BelongsTo_should_locate_property() {
			var hasMany = (BelongsToMapping<Customer, Company>)mapping.BelongsTo(c => c.Company);
			Assert.That(hasMany.Property, Is.EqualTo(typeof(Customer).GetProperty("Company")));
		}

		[Test]
		public void Should_create_instance_of_BelongsToMapping() {
			Assert.IsInstanceOf<BelongsToMapping<Customer, Company>>(mapping.BelongsTo(x => x.Company));
		}

		[Test]
		public void Should_throw_when_a_non_member_Expression_passed_to_BelongsTo() {
			Assert.Throws<ArgumentException>(() => mapping.BelongsTo(x => "foo"));
		}

		[Test]
		public void Should_store_custom_property() {
			mapping.CustomProperties["foo"] = "bar";
			Assert.That(mapping.CustomProperties["foo"], Is.EqualTo("bar"));
		}

		[Test]
		public void Map_Should_not_throw_with_private_property() {
			mapping.Map(Private.Field<Customer, string>("privateField"));
		}

		[Test]
		public void HasMany_should_not_throw_with_private_property() {
			mapping.HasMany(Private.Field<Customer, IList<Order>>("privateOrders"));
		}

		[Test]
		public void HasOne_should_not_throw_with_private_property() {
			mapping.HasOne(Private.Field<Customer, Company>("privateCompany"));
		}

		[Test]
		public void BelongsTo_should_not_throw_with_private_property() {
			mapping.BelongsTo(Private.Field<Customer, Company>("privateCompany"));
		}

		[Test]
		public void DiscriminateOnProperty_should_throw() {
			Assert.Throws<ArgumentNullException>(() => mapping.DiscriminateOnProperty<string>(null));
		}

		[Test]
		public void DiscriminateOnProperty_should_create_DiscriminatorMapping() {
			Assert.IsInstanceOf<DiscriminatorMapping<Customer, int>>(
				mapping.DiscriminateOnProperty(x => x.CustomerType)	
			);
		}

		[Test]
		public void DiscriminateOnProperty_should_add_column_mapping() {
			mapping.DiscriminateOnProperty(x => x.CustomerType);
			mapping
				.CastTo<IMapping>()
				.Mappings
				.OfType<IColumnMapping>()
				.Any(x => x.Property == typeof(Customer).GetProperty("CustomerType"))
				.ShouldBeTrue();
		}

		[Test]
		public void DiscriminateOnProperty_should_modify_existing_mapping_if_column_already_defined() {
			mapping.Map(x => x.CustomerType);
			mapping.DiscriminateOnProperty(x => x.CustomerType);
			mapping
				.CastTo<IMapping>()
				.Mappings
				.OfType<IColumnMapping>()
				.Count(x => x.Property == typeof(Customer).GetProperty("CustomerType"))
				.ShouldEqual(1);
		}


		[Test]
		public void DiscriminateOnProperty_should_set_IsDiscriminator_to_true() {
			var columnMap = mapping.Map(x => x.CustomerType);
			mapping.DiscriminateOnProperty(x => x.CustomerType);
			columnMap.Attributes["IsDiscriminator"].ShouldEqual("true");
		}
	}
}