namespace FluentLinqToSql.ActiveRecord {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using FluentLinqToSql.ActiveRecord.Conventions;
	using FluentLinqToSql.Mappings;

	public class AutoMappingSource : FluentMappingSource {
		private readonly Predicate<Type> mappedTypeSelector;
		private readonly MappingConventions conventions;

		public AutoMappingSource(Predicate<Type> mappedTypeSelector, MappingConventions conventions)
			: base("AutoMapping") {
			this.mappedTypeSelector = mappedTypeSelector;
			this.conventions = conventions;
		}

		public override FluentMappingSource AddFromAssembly(Assembly assembly) {
			return AddTypes(assembly.GetExportedTypes());
		}

		public AutoMappingSource AddTypes(params Type[] types) {
			return AddTypes((IEnumerable<Type>)types);
		}

		public AutoMappingSource AddTypes(IEnumerable<Type> types) {
			var maps = from type in types
			           where mappedTypeSelector(type)
			           select new AutoMapping(type, conventions);

			foreach (var map in maps) {
				AddMapping(map);
			}

			return this;
		}
	}
}