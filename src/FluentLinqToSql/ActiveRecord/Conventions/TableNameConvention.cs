namespace FluentLinqToSql.ActiveRecord.Conventions {
	using System;
	using System.Data.Linq.Mapping;

	public class TableNameConvention {
		public virtual string GetTableName(Type type) {

			var attr = (TableAttribute)Attribute.GetCustomAttribute(type, typeof(TableAttribute));
			
			if(attr != null && !string.IsNullOrEmpty(attr.Name)) {
				return attr.Name;
			}

			return type.Name;
		}
	}
}