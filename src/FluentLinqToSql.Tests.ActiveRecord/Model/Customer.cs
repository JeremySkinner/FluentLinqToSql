namespace FluentLinqToSql.Tests.ActiveRecord.Model {
	using FluentLinqToSql.ActiveRecord;

	public class Customer : ActiveRecord<Customer> {
		public int Id { get; set; }
		public string Name { get; set; }
	}
}