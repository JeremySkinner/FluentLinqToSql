namespace FluentLinqToSql.ActiveRecord.Conventions {
	using System;
	using System.Collections.Generic;
	using System.Data.Linq.Mapping;
	using System.Linq;
	using FluentLinqToSql.Mappings;

	public class PrimaryKeyBuilderConvention {

		public virtual IEnumerable<IColumnMapping> CreateColumnMappings(Type type, IEnumerable<ColumnMetaData> mappableProperties) {
			var pkMembers = GetPrimaryKeyColumns(type, mappableProperties);

			return pkMembers
				.Select(meta => CreateColumnMapping(meta))
				.ToList();
		}

		protected virtual IColumnMapping CreateColumnMapping(ColumnMetaData meta) {
			var map = new ColumnMapping(meta.Member);
			MappedPropertiesConvention.CopyAttributeToMapping(meta.Attribute, map);
			return map;
		}

		protected List<ColumnMetaData> FindMembersWithAppropriateColumnAttribute(IEnumerable<ColumnMetaData> mappedProperties) {
			var query = from member in mappedProperties
			            where member.Attribute != null
			            where member.Attribute.IsPrimaryKey
			            select member;

			return query.ToList();
		}

		public virtual IEnumerable<ColumnMetaData> GetPrimaryKeyColumns(Type type, IEnumerable<ColumnMetaData> mappableProperties) {
			var pkMembers = FindMembersWithAppropriateColumnAttribute(mappableProperties);

			if (pkMembers.Count == 0) {
				var id = mappableProperties.SingleOrDefault(x => x.Member.Name == "Id");

				if (id == null) {
					throw new InvalidOperationException(string.Format("Could not find a public instance property called 'Id' on type '{0}' with public a public getter and setter.", type.Name));
				}


				if (id.Attribute == null) {
					id.Attribute = new ColumnAttribute() { IsDbGenerated = true, IsPrimaryKey = true };
				}
				else if (!id.Attribute.IsPrimaryKey) {
					id.Attribute.IsPrimaryKey = true;
				}

				pkMembers.Add(id);
			}

			return pkMembers;
		}
	}
}