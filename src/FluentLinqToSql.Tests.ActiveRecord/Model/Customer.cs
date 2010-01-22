namespace FluentLinqToSql.Tests.ActiveRecord.Model {
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using FluentLinqToSql.ActiveRecord;

//	[Table]
	public class Customer : ActiveRecord<Customer> {
//		[Column(IsPrimaryKey = true, IsDbGenerated = true)]
		public int Id { get; set; }

//		[Column]
		public string Name { get; set; }

		public Customer() {
			orders = new EntitySet<Order>(o => o.Customer = this, o => o.Customer= null);
		}

		private EntitySet<Order> orders;

		public EntitySet<Order> Orders {
			get { return orders; }
			set { orders.Assign(value); }
		}
	}
}