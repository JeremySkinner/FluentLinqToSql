namespace FluentLinqToSql.Tests.ActiveRecord.Model
{
	using System.Data.Linq;
	using FluentLinqToSql.ActiveRecord;

	public class Order : ActiveRecord<Order>
	{
		public int Id { get; set; }
		public string ProductName { get; set; }

		private EntityRef<Customer> customer;
		public Customer Customer {
			get { return customer.Entity; }
			set { customer.Entity = value; }
		}

		public decimal Amount { get; set; }
		public int CustomerId { get; set; }
	}
}