namespace FluentLinqToSql.ActiveRecord {
	using System;

	public interface IScopeStorage {
		void StoreScope(ContextScopeBase scope, Type contextType);
		ContextScopeBase GetScope(Type contextType);
		void RemoveScope(Type contextType);
	}
}