namespace FluentLinqToSql.ActiveRecord {
	using System;
	using System.Web;

	public class DefaultScopeStorage : IScopeStorage {
		readonly IScopeStorage webStorage = new WebScopeStorage();
		readonly IScopeStorage staticStorage = new StaticScopeStorage();

		public void StoreScope(ContextScopeBase scope, Type contextType) {
			Storage.StoreScope(scope, contextType);
		}

		public ContextScopeBase GetScope(Type contextType) {
			return Storage.GetScope(contextType);
		}

		public void RemoveScope(Type contextType) {
			Storage.RemoveScope(contextType);
		}

		IScopeStorage Storage {
			get { return HttpContext.Current != null ? webStorage : staticStorage; }
		}
	}
}