namespace FluentLinqToSql.Mappings {
	using System.Linq;
	using System.Xml.Linq;
	using Internal;

	public class FunctionReturnTypeMapping<T> : TypeMapping<T> {
		public override System.Collections.Generic.IEnumerable<XElement> ToXml() {
			var typeElement = base.ToXml().Single();

			yield return new LinqElement(Constants.ElementType, 
				typeElement.Attributes(),
				typeElement.Elements()
			);
		}
	}
}