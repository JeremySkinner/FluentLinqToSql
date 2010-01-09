namespace FluentLinqToSql.Mappings {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Xml.Linq;
	using Internal;

	public class DiscriminatorMapping<T, TDiscriminator> : IDiscriminatorMapping<T, TDiscriminator> {
		private readonly IDictionary<string, object> customProperties = new Dictionary<string, object>();
		private readonly IList<IMapping> mappings = new List<IMapping>();
		private IMapping parentMapping;

		public DiscriminatorMapping(IMapping parentMapping) {
			this.parentMapping = parentMapping;
		}

		public IDiscriminatorMapping<T, TDiscriminator> SubClass<TSubClass>(TDiscriminator discriminatorValue, Action<SubClassMapping<TSubClass, TDiscriminator>> mapping) where TSubClass : T {
			var subclass = new SubClassMapping<TSubClass, TDiscriminator>(discriminatorValue);
			mappings.Add(subclass);

			mapping(subclass);

			return this;
		}

		public IDiscriminatorMapping<T, TDiscriminator> BaseClassDiscriminatorValue(TDiscriminator discriminator) {
			parentMapping.CustomProperties[Constants.InheritanceCode] = discriminator;
			return this;
		}

		public IDictionary<string, object> CustomProperties {
			get { return customProperties; }
		}

		public IEnumerable<XElement> ToXml() {
			//base class should be the inheritance default if it is not explicitly specified on one of the subclasses.

			if (!mappings.Any(x => x.CustomProperties.ContainsKey(Constants.IsInheritanceDefault))) {
				parentMapping.CustomProperties[Constants.IsInheritanceDefault] = "true";
			}

			return mappings.SelectMany(x => x.ToXml());
		}

		IList<IMapping> IDiscriminatorMapping.SubClassMappings {
			get { return mappings; }
		}
	}
}