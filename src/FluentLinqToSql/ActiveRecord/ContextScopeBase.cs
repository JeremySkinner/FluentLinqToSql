namespace FluentLinqToSql.ActiveRecord {
	using System;
	using System.Data.Linq;
	using System.Transactions;

	public abstract class ContextScopeBase : IDisposable {
		readonly Type contextType;
		bool commitVetoed;
		readonly DataContext context;

		protected ContextScopeBase(Type contextType) {
			this.contextType = contextType;
			context = CreateContext();
			ScopeStorage.StoreScope(this, contextType);
		}

		public void Dispose() {
			try {
				if (!commitVetoed) {
					using (var tx = new TransactionScope()) {
						context.SubmitChanges();
						tx.Complete();
					}
				}
			}
			finally {
				ScopeStorage.RemoveScope(contextType);
				context.Dispose();
			}
		}

		protected static IScopeStorage ScopeStorage {
			get { return ActiveRecordConfiguration.Current.ScopeStorage; }
		}

		protected abstract DataContext CreateContext();

		public DataContext Context {
			get { return context; }
		}

		public void VetoCommit() {
			commitVetoed = true;
		}
	}

	public abstract class ContextScopeBase<TContext> : ContextScopeBase where TContext : DataContext {
		protected ContextScopeBase() : base(typeof(TContext)) {
		}

		public new TContext Context {
			get { return (TContext) base.Context; }
		}

		public static ContextScopeBase<TContext> Current {
			get {
				var currentScope = ScopeStorage.GetScope(typeof(TContext));

				if (currentScope == null) {
					throw new InvalidOperationException("There is no current context scope. Did you forget to call ContextScope.Begin() ?");
				}

				return (ContextScopeBase<TContext>) currentScope;
			}
		}

		protected override DataContext CreateContext() {
			return ActiveRecordConfiguration.Current.CreateContext(typeof(TContext));
		}
	}

	public sealed class ContextScope : ContextScopeBase<DataContext> {
		public static ContextScope Begin() {
			return new ContextScope();
		}
	}
}