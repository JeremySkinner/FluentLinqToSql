namespace FluentLinqToSql {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using FluentLinqToSql.Mappings;
	using FluentLinqToSql.Modifications;

	public class ConventionMappingSource : FluentMappingSource {
		private readonly Predicate<Type> mappedTypeSelector;
		private readonly MappingConventions conventions;

		public ConventionMappingSource(Predicate<Type> mappedTypeSelector, MappingConventions conventions)
			: base("AutoMapping") {
			this.mappedTypeSelector = mappedTypeSelector;
			this.conventions = conventions;

			AddModification(UseFieldForAssociationStorage.LowercaseFirstCharacter);
		}

		public override FluentMappingSource AddFromAssembly(Assembly assembly) {
			return AddTypes(assembly.GetExportedTypes());
		}

		public ConventionMappingSource AddTypes(params Type[] types) {
			return AddTypes((IEnumerable<Type>)types);
		}

		public ConventionMappingSource AddTypes(IEnumerable<Type> types) {
			var maps = from type in types
					   where mappedTypeSelector(type)
					   select new ConventionMapping(type, types, conventions);

			foreach (var map in maps) {
				AddMapping(map);
			}

			return this;
		}
	}
}