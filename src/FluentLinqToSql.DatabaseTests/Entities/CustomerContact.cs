namespace FluentLinqToSql.DatabaseTests.Entities {
	using System.Data.Linq;

	public class CustomerContact {
		public int Id { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }

		private EntityRef<Customer> customer; 

		public Customer Customer {
			get { return customer.Entity; }
			set { customer.Entity = value; }
		}

		#region Equality
		public bool Equals(CustomerContact obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.Id == Id;
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof(CustomerContact)) return false;
			return Equals((CustomerContact)obj);
		}

		public override int GetHashCode() {
			return Id;
		}

		#endregion
	}
}