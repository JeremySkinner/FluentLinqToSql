namespace FluentLinqToSql.DatabaseTests {
	using System.Linq;
	using Entities;
	using FluentLinqToSql.Mappings;
	using NUnit.Framework;
	using TestHelper;

	[TestFixture]
	public class SimpleMappingTester : DatabaseTest {
		[Test]
		public void Should_map_simple_properties() {
			using(var test = Verify<Customer>()) {
				test.TestProperty(x => x.Name, "Jeremy");
			}
		}

		[Test]
		public void MappingTester_should_throw_for_incorrectly_mapped_proprty() {
			var brokenMappings = new TestMappingSource();

			//Remove the Customer.Name mapping. 
			var customerMapping= brokenMappings.OfType<Mapping<Customer>>().Cast<IMapping>().Single();
			customerMapping.Mappings.RemoveAt(1);
			
			DataContextFactory = () => new TestDataContext(brokenMappings);

			Assert.Throws<MappingException>(() => {
				using (var test = Verify<Customer>()) {
					test.TestProperty(x => x.Name, "Jeremy");
				}
			});
		}
	}
}