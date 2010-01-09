namespace FluentLinqToSql.ActiveRecord {
	using System.Web;

	public class ScopeStorageModule : IHttpModule {
		public void Init(HttpApplication context) {
			context.BeginRequest += delegate {
				ContextScope.Begin();
			};

			context.EndRequest += delegate {
				ContextScope.Current.Dispose();
			};
		}

		public void Dispose() { }
	}
}