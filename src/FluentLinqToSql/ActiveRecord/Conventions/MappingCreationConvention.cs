namespace FluentLinqToSql.ActiveRecord.Conventions {
	using System;
	using FluentLinqToSql;
	using FluentLinqToSql.Mappings;

	public class MappingCreationConvention {
		public virtual IMapping CreateMapping(Type type) {
			var mappingType = typeof(Mapping<>).MakeGenericType(type);
			return (IMapping)Activator.CreateInstance(mappingType);
		}
	}
}