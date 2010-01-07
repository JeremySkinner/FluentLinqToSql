namespace FluentLinqToSql.DatabaseTests {
	using Entities;
	using Modifications;

	public class TestMappingSource : FluentMappingSource {
		public TestMappingSource() : base("FluentLinqToSql") {
			AddFromAssemblyContaining<Customer>();
			AddModification(UseFieldForAssociationStorage.LowercaseFirstCharacter);
		}
	}
}