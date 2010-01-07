namespace FluentLinqToSql.Tests {
	using System.Linq;
	using System.Xml.Linq;
	using Mappings;
	using NUnit.Framework;

	[TestFixture]
	public class BelongsToTester {
		private BelongsToMapping<Order, Customer> mapping;

		[SetUp]
		public void Setup() {
			mapping = new BelongsToMapping<Order, Customer>(typeof(Order).GetProperty("Customer"));
		}

		[Test]
		public void Should_automatically_set_IsForeignKey() {
			MappingXml.ShouldHaveAttribute("IsForeignKey", "true");
		}

		protected XElement MappingXml {
			get { return mapping.CastTo<IPropertyMapping>().ToXml().Single(); }
		}
	}
}