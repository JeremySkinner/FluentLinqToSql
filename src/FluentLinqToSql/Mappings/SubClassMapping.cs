namespace FluentLinqToSql.Mappings {
	using System.Linq;
	using System.Xml.Linq;

	public class SubClassMapping<T, TDiscriminator> : TypeMapping<T> {

		public SubClassMapping(TDiscriminator discriminatorValue) {
			CustomProperties["InheritanceCode"] = discriminatorValue;
		}

		public void InheritanceDefault() {
			CustomProperties["IsInheritanceDefault"] = "true";
		}
	}
}