namespace FluentLinqToSql.ActiveRecord {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Xml.Linq;
	using FluentLinqToSql.ActiveRecord.Conventions;
	using FluentLinqToSql.Internal;
	using FluentLinqToSql.Mappings;

	public class AutoMapping : IMapping {
		readonly IMapping innerMapping;
		readonly Type type;
		readonly MappingConventions conventions;
		readonly string tableName;
		IEnumerable<IMemberMetaData> metadata;

		public AutoMapping(Type type, MappingConventions conventions) {
			this.type = type;
			this.conventions = conventions;
			this.metadata = conventions.MetaDataBuilder.GetMetaData(type);

			tableName = conventions.TableName.GetTableName(type);
			innerMapping = conventions.MappingCreator.CreateMapping(type);

			innerMapping.CustomProperties[Constants.TableName] = tableName;

			LoadPropertyMappings();
		}

		private void LoadPropertyMappings() {
			var columnMeta = metadata.OfType<ColumnMetaData>().ToList();
			
			var keyMappings = conventions.PrimaryKeyBuilder.CreateColumnMappings(type, columnMeta);

			var propsWithoutKeys = from prop in columnMeta
			                       let keyProperties = keyMappings.Select(x => x.Property)
			                       where !keyProperties.Contains(prop.Member)
			                       select prop;

			var propertyMappings = conventions.MappedPropertiesBuilder.CreateColumnMappings(type, propsWithoutKeys);

			foreach(var idMapping in keyMappings.Concat(propertyMappings)) {
				Mappings.Add(idMapping);
			}
		}

		public IEnumerable<XElement> ToXml() {
			return innerMapping.ToXml();
		}

		public IDictionary<string, object> CustomProperties {
			get { return innerMapping.CustomProperties; }
		}

		public Type MappedType {
			get { return innerMapping.MappedType; }
		}

		public IList<IElementMapping> Mappings {
			get { return innerMapping.Mappings; }
		}
	}
}