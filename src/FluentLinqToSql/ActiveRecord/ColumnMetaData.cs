namespace FluentLinqToSql.ActiveRecord {
	using System;
	using System.Data.Linq.Mapping;
	using System.Reflection;

	public interface IMemberMetaData {
		MemberInfo Member { get; }
		Attribute Attribute { get; }
	}

	public class ColumnMetaData : IMemberMetaData {
		public MemberInfo Member { get; private set; }
		public ColumnAttribute Attribute { get; internal set; }

		public ColumnMetaData(MemberInfo member, ColumnAttribute attribute) {
			Member = member;
			Attribute = attribute;
		}

		Attribute IMemberMetaData.Attribute { get { return Attribute; } }
	}

	public class AssociationMetaData : IMemberMetaData {
		public MemberInfo Member { get; private set; }
		public AssociationAttribute Attribute { get; private set; }

		public AssociationMetaData(MemberInfo member, AssociationAttribute attribute) {
			Member = member;
			Attribute = attribute;
		}

		Attribute IMemberMetaData.Attribute { get { return Attribute; } }

	}
}