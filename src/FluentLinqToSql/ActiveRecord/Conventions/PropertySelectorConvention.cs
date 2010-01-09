namespace FluentLinqToSql.ActiveRecord.Conventions {
	using System;
	using System.Collections.Generic;
	using System.Data.Linq.Mapping;
	using System.Linq;
	using FluentLinqToSql.Mappings;

	public class MappedPropertiesConvention {
		public virtual IEnumerable<IColumnMapping> CreateColumnMappings(Type type, IEnumerable<ColumnMetaData> properties) {
			return properties
				.Select(prop =>  CreateColumnMapping(prop) )
				.ToList();
		}

		protected virtual IColumnMapping CreateColumnMapping(ColumnMetaData column) {
			var map = new ColumnMapping(column.Member);
			CopyAttributeToMapping(column.Attribute, map);
			return map;
		}

		public static void CopyAttributeToMapping(ColumnAttribute attribute, IColumnMapping mapping) {
			if (attribute == null) return;

			if (attribute.IsDbGenerated) mapping.DbGenerated();
			if (attribute.IsPrimaryKey) mapping.PrimaryKey();
			if (attribute.IsVersion) mapping.Version();
			if (! attribute.CanBeNull) mapping.NotNull();

			if (!string.IsNullOrEmpty(attribute.DbType)) mapping.DbType(attribute.DbType);
			if (!string.IsNullOrEmpty(attribute.Name)) mapping.Named(attribute.Name);
			if (!string.IsNullOrEmpty(attribute.Expression)) mapping.Expression(attribute.Expression);
			if (!string.IsNullOrEmpty(attribute.Storage)) mapping.Storage(attribute.Storage);


			mapping.AutoSync(attribute.AutoSync);
			mapping.UpdateCheck(attribute.UpdateCheck);

			//TODO: Discriminator
		}
	}
}