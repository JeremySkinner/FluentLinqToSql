namespace FluentLinqToSql.Tests.ActiveRecord.Model {
	using System.Data.Linq.Mapping;
	using FluentLinqToSql.ActiveRecord;

	[Table(Name = "Customer2")]
	public class CustomisedCustomer : ActiveRecord<CustomisedCustomer> {

		[Column(IsPrimaryKey = true, IsDbGenerated = true, Name = "Id")]
		public int CustomerId { get; set; }
		
		[Column(Name = "CustomerName")]
		public string Name { get; set; }

		[NotMapped]
		public string NotMapped { get; set; }
	}
}