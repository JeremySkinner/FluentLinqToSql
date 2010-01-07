namespace FluentLinqToSql.DatabaseTests.Mappings {
	using Entities;

	public class FunctionMappings : FunctionMapping<TestDataContext> {
		public FunctionMappings() {
			Map(x => x.CustomersByName(	
				Parameter<string>(param => param.Named("name").DbType("nvarchar(250)"))
			)).Composable();

			Map(x => x.MeaningOfLife()).Named("ScalarFunction");
			Map(x => x.CustomersAndOrders())
				.ElementType<Customer>()
				.ElementType<Order>();

			Map(x => x.CountCustomerOrders())
				.ElementType<CustomerOrderCount>(type => {
					type.Map(x => x.Name).Named("CustomerName");
					type.Map(x => x.NumberOfOrders);
				});
		}
	}
}