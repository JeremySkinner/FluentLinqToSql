namespace FluentLinqToSql.DatabaseTests.Mappings {
	using Entities;

	public class CustomerContactMapping : Mapping<CustomerContact> {
		public CustomerContactMapping() {
			Named("CustomerContacts");
			Map(x => x.Id).PrimaryKey();
			Map(x => x.Email).DbType("nvarchar(250)");
			Map(x => x.PhoneNumber).DbType("nvarchar(50)");
			BelongsTo(x => x.Customer);
		}
	}
}