namespace FluentLinqToSql.DatabaseTests.Mappings {
	using Entities;
	using FluentLinqToSql.Mappings;

	public class CustomerMapping : Mapping<Customer> {
		public CustomerMapping() {
			Named("Customers");
			Identity(x => x.Id);
			Map(x => x.Name).DbType("nvarchar(50)");
			HasMany(x => x.Orders).OtherKey(x => x.CustomerId);
			HasOne(x => x.ContactDetails);
			DiscriminateOnProperty(x => x.CustomerType)
				.BaseClassDiscriminatorValue(0)
				.SubClass<PreferredCustomer>(2, preferredCustomer => {
					preferredCustomer.Map(x => x.Discount);
				});
		}
	}
}