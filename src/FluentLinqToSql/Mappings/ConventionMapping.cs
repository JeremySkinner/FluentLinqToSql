namespace FluentLinqToSql.Mappings {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Xml.Linq;
	using FluentLinqToSql.Internal;

	public class ConventionMapping : IMapping {
		readonly IMapping innerMapping;
		readonly Type type;
		private readonly IEnumerable<Type> otherTypes;
		readonly MappingConventions conventions;
		readonly string tableName;
//		IEnumerable<IMemberMetaData> metadata;

		public ConventionMapping(Type type, IEnumerable<Type> otherTypes, MappingConventions conventions) {
			this.type = type;
			this.otherTypes = otherTypes;
			this.conventions = conventions;
//			this.metadata = conventions.MetaDataBuilder.GetMetaData(type);

//			tableName = conventions.TableName.GetTableName(type);
			tableName = conventions.TableNameSelector(type);
			innerMapping = conventions.MappingCreator(type);

			innerMapping.CustomProperties[Constants.TableName] = tableName;

			LoadPropertyMappings();
		}

		private void LoadPropertyMappings() {
			var ids = conventions.PrimaryKeySelector(type);
			var manyToOnes = conventions.BelongsToSelector(type, otherTypes);
			var oneToMany = conventions.HasManySelector(type, otherTypes);
			var otherMappedProperties = ids.Select(x => x.Property).Concat(manyToOnes.Select(x => x.Property)).Concat(oneToMany.Select(x => x.Property));
			var columns = conventions.PropertySelector(type, otherMappedProperties);

			foreach(var id in ids) {
				Mappings.Add(id);
			}

			foreach(var manyToOne in manyToOnes) {
				Mappings.Add(manyToOne);
			}

			foreach (var otm in oneToMany) {
				Mappings.Add(otm);
			}


			foreach(var column in columns) {
				Mappings.Add(column);
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