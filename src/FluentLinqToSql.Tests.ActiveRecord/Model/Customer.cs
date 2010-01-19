namespace FluentLinqToSql.Tests.ActiveRecord.Model {
	using System.ComponentModel.DataAnnotations;
	using System.Data.Linq.Mapping;
	using FluentLinqToSql.ActiveRecord;

//	[Table]
	public class Customer : ActiveRecord<Customer> {
//		[Column(IsPrimaryKey = true, IsDbGenerated = true)]
		public int Id { get; set; }

//		[Column]
		public string Name { get; set; }
	}
}