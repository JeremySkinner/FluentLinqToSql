namespace FluentLinqToSql.DatabaseTests.Mappings {
	using System.Data.Linq.Mapping;
	using Entities;

	public class OrderMapping : Mapping<Order> {
		public OrderMapping() {
			Named("Orders");
			Identity(x => x.Id);
			Map(x => x.CustomerId);
			Map(x => x.OrderDate);
			BelongsTo(x => x.Customer).ThisKey(x => x.CustomerId);
		}
	}
}