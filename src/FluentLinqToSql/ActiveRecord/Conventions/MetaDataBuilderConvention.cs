namespace FluentLinqToSql.ActiveRecord.Conventions {
	using System;
	using System.Collections.Generic;
	using System.Data.Linq.Mapping;
	using System.Linq;
	using System.Reflection;

	public class MetaDataBuilderConvention {
		public virtual IEnumerable<IMemberMetaData> GetMetaData(Type type) {
			var members = GetMembers(type);
			var meta = from member in members
			           let memberMeta = GetMetaDataForMember(member)
			           where memberMeta != null
			           select memberMeta;


			return meta.ToList();
		}

		protected virtual IEnumerable<MemberInfo> GetMembers(Type type) {
			var props = from property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
			            select property;

			return props.Cast<MemberInfo>();
		}

		protected virtual IMemberMetaData GetMetaDataForMember(MemberInfo member) {
			var columnAttribute = (ColumnAttribute)Attribute.GetCustomAttribute(member, typeof(ColumnAttribute));

			if(columnAttribute != null) {
				return new ColumnMetaData(member, columnAttribute);
			}

			var associationAttribute = (AssociationAttribute)Attribute.GetCustomAttribute(member, typeof(AssociationAttribute));

			if(associationAttribute != null) {
				return new AssociationMetaData(member, associationAttribute);
			}

			var notMapped = Attribute.IsDefined(member, typeof(NotMappedAttribute));

			if(notMapped) {
				return null;
			}

			//TODO: auto Determine whether member is Association or Column
			return new ColumnMetaData(member, null);
		}


	}
}