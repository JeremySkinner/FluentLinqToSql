namespace FluentLinqToSql.Tests {
	using System.Linq;
	using Mappings;
	using Modifications;
	using NUnit.Framework;

	[TestFixture]
	public class ChangeUpdateCheckToNeverModificationTester {
		private IMappingModification modification;

		[SetUp]
		public void Setup() {
			modification = new ChangeUpdateCheckToNever();
		}

		[Test]
		public void Should_change_update_check_to_never() {
			var mapping = new CustomerMapping();
			mapping.Map(x => x.Surname);

			modification.ApplyTo(mapping);

			var columnMapping = ((IMapping)mapping).Mappings.Single().CastTo<IColumnMapping>();
			Assert.That(columnMapping.Attributes["UpdateCheck"], Is.EqualTo("Never"));
		}

	}
}