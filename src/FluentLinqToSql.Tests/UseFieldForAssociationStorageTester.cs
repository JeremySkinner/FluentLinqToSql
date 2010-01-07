namespace FluentLinqToSql.Tests {
	using System.Linq;
	using Mappings;
	using Modifications;
	using NUnit.Framework;

	[TestFixture]
	public class UseFieldForAssociationStorageTester {
		private IMappingModification modification;
		private Mapping<Customer> mapping;

		[SetUp]
		public void Setup() {
			modification = new UseFieldForAssociationStorage(property => "foo");
			mapping = new CustomerMapping();
			mapping.HasMany(x => x.Orders);
			mapping.HasOne(x => x.Company);
			mapping.BelongsTo(x => x.Company);
			mapping.HasOne(x => x.Company).Storage("baz");
		}

		[Test]
		public void Should_set_storage_for_HasMany_association() {
			modification.ApplyTo(mapping);
			var storage = mapping
				.CastTo<IMapping>()
				.Mappings
				.OfType<IHasManyMapping>()
				.First()
				.Attributes["Storage"];

			Assert.That(storage, Is.EqualTo("foo"));
		}

		[Test]
		public void Should_set_storage_for_HasOne_association() {
			modification.ApplyTo(mapping);
			var storage = mapping
				.CastTo<IMapping>()
				.Mappings
				.OfType<IHasOneMapping>()
				.First()
				.Attributes["Storage"];

			Assert.That(storage, Is.EqualTo("foo"));
		}

		[Test]
		public void Should_set_storage_for_BelongsTo_association() {
			modification.ApplyTo(mapping);
			var storage = mapping
				.CastTo<IMapping>()
				.Mappings
				.OfType<IBelongsToMapping>()
				.First()
				.Attributes["Storage"];
			Assert.That(storage, Is.EqualTo("foo"));
		}


		[Test]
		public void Should_not_overwrite_explicit_storage() {
			modification.ApplyTo(mapping);
			var storage = mapping
				.CastTo<IMapping>()
				.Mappings
				.OfType<IHasOneMapping>()
				.Last()
				.Attributes["Storage"];

			Assert.That(storage, Is.EqualTo("baz"));
		}

		[Test]
		public void LowercaseFirstCharacter_should_create_modification_that_will_lowercase_first_character() {
			var modification = UseFieldForAssociationStorage.LowercaseFirstCharacter;
			string fieldName = modification.NameSpecifier(new HasManyMapping<Customer, Order>(typeof(Customer).GetProperty("Orders")));
			Assert.That(fieldName, Is.EqualTo("orders"));
		}

		[Test]
		public void UnderscorePrefixLowercaseFirstCharacter_should_create_modification_that_will_prefix_first_character_with_underscore() {
			var modification = UseFieldForAssociationStorage.UnderscorePrefixLowercaseFirstCharacter;
			string fieldName = modification.NameSpecifier(new HasManyMapping<Customer, Order>(typeof(Customer).GetProperty("Orders")));
			Assert.That(fieldName, Is.EqualTo("_orders"));
		}
	}
}