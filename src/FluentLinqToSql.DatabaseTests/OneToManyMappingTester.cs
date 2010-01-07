namespace FluentLinqToSql.DatabaseTests {
	using System;
	using System.Linq;
	using Entities;
	using FluentLinqToSql.Mappings;
	using NUnit.Framework;
	using TestHelper;

	[TestFixture]
	public class OneToManyMappingTester : DatabaseTest {
		[Test]
		public void Should_map_one_to_many_collection() {
			using(var test = Verify<Customer>()) {
				test.TestCollection(x => x.Orders, new Order { OrderDate = DateTime.Now });
			}
		}

		[Test]
		public void MappingTester_should_throw_for_incorrectly_mapped_collection() {
			var brokenMappings = new TestMappingSource();
			//Remove the customer.Orders mapping.
			var customerMapping = brokenMappings.OfType<Mapping<Customer>>().Cast<IMapping>().Single();
			customerMapping.Mappings.RemoveAt(2);

			DataContextFactory = () => new TestDataContext(brokenMappings);

			Assert.Throws<MappingException>(() => {
				using (var test = Verify<Customer>()) {
					test.TestCollection(x => x.Orders, new Order() { OrderDate = DateTime.Now });
				}
			});
		}
	}
}