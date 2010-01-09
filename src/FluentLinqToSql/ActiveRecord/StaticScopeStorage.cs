namespace FluentLinqToSql.ActiveRecord {
	using System;
	using System.Collections.Generic;

	public class StaticScopeStorage : IScopeStorage {
		static readonly Dictionary<Type, ContextScopeBase> storage = new Dictionary<Type, ContextScopeBase>();

		public void StoreScope(ContextScopeBase scope, Type contextType) {
			storage[contextType] = scope;
		}

		public ContextScopeBase GetScope(Type contextType) {
			ContextScopeBase scope = null;
			storage.TryGetValue(contextType, out scope);
			return scope;
		}

		public void RemoveScope(Type contextType) {
			storage[contextType] = null;
		}
	}
}