namespace FluentLinqToSql.ActiveRecord {
	using System;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class NotMappedAttribute : Attribute {
		
	}
}