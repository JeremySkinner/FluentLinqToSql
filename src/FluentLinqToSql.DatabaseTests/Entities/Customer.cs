namespace FluentLinqToSql.DatabaseTests.Entities {
	using System.Collections.Generic;
	using System.Data.Linq;

	public class Customer {
		public int Id { get; set; }
		public string Name { get; set; }
		public int CustomerType { get; set; }

		private readonly EntitySet<Order> orders;

		public Customer() {
			orders = new EntitySet<Order>(order => order.Customer = this, order => order.Customer = null);
		}

		public IList<Order> Orders {
			get {
				return orders;
			}
			set {
				orders.Assign(value);
			}
		}

		private EntityRef<CustomerContact> contactDetails;

		public CustomerContact ContactDetails {
			get { return contactDetails.Entity; }
			set { 
				contactDetails.Entity = value;
				if(value != null) value.Customer = this;
			}
		}

		#region Equality

		public bool Equals(Customer obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			
			return (obj.Id == Id && Id != 0) ? true : base.Equals(obj);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof(Customer)) return false;
			return Equals((Customer)obj);
		}

		public override int GetHashCode() {
			return Id;
		}

		#endregion
	}
}