namespace Demo {
	using FluentLinqToSql;

	public class CustomerMapping : Mapping<Customer> {
		public CustomerMapping() {
			Named("Customers");
			Identity(x => x.Id);
			Map(x => x.Surname).Named("LastName");
			Map(x => x.Forename);

			Map(x => x.Id).PrimaryKey().DbGenerated();
		}
	}

}
namespace Demo {
	public class Customer {
		public int Id { get; set; }
		public string Surname { get; set; }
		public string Forename { get; set; }
	}
}

namespace Demo {
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using FluentLinqToSql;

	public class MyDataContext : DataContext {
		private static readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
		private static readonly MappingSource mappings;
 
		static MyDataContext() {
			mappings = new FluentMappingSource("MyDatabase")
				.AddFromAssemblyContaining<CustomerMapping>()
				.CreateMappingSource();
		}
	}

}