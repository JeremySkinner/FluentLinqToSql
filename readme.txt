Fluent Linq to Sql allows you to define mappings for Linq to Sql entities using a fluent interface rather than attributes or XML. 

For discussions, documentation and binary releases please visit the CodePlex site:
http://fluentlinqtosql.codeplex.com

This project was inspired by Fluent NHibernate (http://fluentnhibernate.org).

A simple example:

public class CustomerMapping : Mapping<Customer> {
   public CustomerMapping() {
      Identity(customer => customer.Id);
      Map(customer => customer.Name);
      HasMany(customer => customer.Orders).OtherKey(order => order.CustomerId);
   }
}

var mappingSource = new FluentMappingSource("Mydatabase");
mappingSource.AddFromAssemblyContaining<CustomerMapping>();

var dataContext = new DataContext("connection-string", mappingSource);
var customers = from c in dataContext.GetTable<Customer>() select c;

Also included in this project is a Linq to Sql based ActiveRecord implementation:

public class Customer {
  public int Id { get; set; } //assumes public property named "Id" is PK
  public string Name { get; set; } //all public read/write properties auto-mapped to db cols
}
 
//usage:
using(ContextScope.Begin()) { //in a web app, this can be transparent using a scope per request
  var customer = new Customer { Name = "Jeremy" };
  customer.Save();
} //changes committed on scope disposal
 
using(ContextScope.Begin()) {
  var cust = Customer.FindById(1); //normal AR-style operations
}
 
//testability:
var fakeData = new[] { new Customer { Id = 1 }, new Customer { Id = 2 } };
using(Customer.Fake(fakeData)) { //replaces underlying data access with in-memory collection
  var cust = Customer.FindById(1);
}

Note that if you want to run the unit tests for this project, 
you'll fist need to create a local SQL Server database called "FluentLinqToSql".
This can be achieved by running BuildDatabase.cmd

Copyright Jeremy Skinner 
http://www.jeremyskinner.co.uk

Licensed under the Apache License 2 
http://www.apache.org/licenses/LICENSE-2.0.html