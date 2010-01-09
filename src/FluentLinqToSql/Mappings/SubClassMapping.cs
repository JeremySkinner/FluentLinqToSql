namespace FluentLinqToSql.Mappings {
	using System.Linq;
	using System.Xml.Linq;
	using Internal;

	public class SubClassMapping<T, TDiscriminator> : TypeMapping<T> {

		public SubClassMapping(TDiscriminator discriminatorValue) {
			CustomProperties[Constants.InheritanceCode] = discriminatorValue;
		}

		public void InheritanceDefault() {
			CustomProperties[Constants.IsInheritanceDefault] = "true";
		}
	}
}