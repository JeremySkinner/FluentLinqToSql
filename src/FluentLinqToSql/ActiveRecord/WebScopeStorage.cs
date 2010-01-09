namespace FluentLinqToSql.ActiveRecord {
	using System;
	using System.Web;

	public class WebScopeStorage : IScopeStorage {
		private const string key = "__ActiveRecordScope__";

		public void StoreScope(ContextScopeBase scope, Type contextType) {
			var key = BuildKey(contextType);
			HttpContext.Current.Items[key] = scope;
		}

		public ContextScopeBase GetScope(Type contextType) {
			var key = BuildKey(contextType);
			return HttpContext.Current.Items[key] as ContextScopeBase;
		}

		public void RemoveScope(Type contextType) {
			var key = BuildKey(contextType);
			HttpContext.Current.Items[key] = null; 
		}

		private static string BuildKey(Type type) {
			return key + type.FullName;
		}
	}
}