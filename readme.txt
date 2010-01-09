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

Note that if you want to run the unit tests for this project, 
you'll fist need to create a local SQL Server database called "FluentLinqToSql".
This can be achieved by running BuildDatabase.cmd

Copyright Jeremy Skinner 
http://www.jeremyskinner.co.uk

Licensed under the Apache License 2 
http://www.apache.org/licenses/LICENSE-2.0.html