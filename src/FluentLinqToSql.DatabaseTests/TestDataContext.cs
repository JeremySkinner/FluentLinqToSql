namespace FluentLinqToSql.DatabaseTests {
	using System;
	using System.Collections.Generic;
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Linq;
	using System.Reflection;
	using Entities;

	public class TestDataContext : DataContext {
		public static string ConnectionString = "Data Source=(local);Initial Catalog=FluentLinqToSql;Persist Security Info=True;Integrated Security=SSPI";
		public TestDataContext(MappingSource mappingSource) : base(ConnectionString, mappingSource) {
			Log = Console.Out;
		}

		public IQueryable<Customer> CustomersByName(string customerName) {
			return CreateMethodCallQuery<Customer>(this, (MethodInfo)MethodInfo.GetCurrentMethod(), customerName);
		}

		public int MeaningOfLife() {
			var result = ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod());
			return (int)result.ReturnValue;
		}

		public IMultipleResults CustomersAndOrders() {
			return (IMultipleResults)ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod()).ReturnValue;
		}

		public IEnumerable<CustomerOrderCount> CountCustomerOrders() {
			return (IEnumerable<CustomerOrderCount>)ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod()).ReturnValue;
		}
	}
}