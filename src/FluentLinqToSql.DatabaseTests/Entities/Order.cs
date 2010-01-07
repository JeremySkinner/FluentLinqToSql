namespace FluentLinqToSql.DatabaseTests.Entities {
	using System;
	using System.Data.Linq;

	public class Order {
		public int Id { get; set; }
		public int CustomerId { get; set; }
		public DateTime OrderDate { get; set; }
		
		private EntityRef<Customer> customer;

		public Customer Customer {
			get { return customer.Entity; }
			set { customer.Entity = value; }
		}
	}
}