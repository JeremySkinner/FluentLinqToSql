namespace FluentLinqToSql.ActiveRecord {
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using FluentLinqToSql.Internal;

	internal interface IDataAccess<T> {
		T FindById(object id);
		IQueryable<T> FindAll();
		void Save(T entity);
		void Delete(T entity);
	}

	internal class DefaultDataAccess<T> : IDataAccess<T> where T : class {
		public T FindById(object id) {
			var context = ContextScope.Current.Context;
			var keyName = GetKeyName(context.GetType());

			if (string.IsNullOrEmpty(keyName)) {
				throw new NotSupportedException("FindById is not supported for entities that do not have a single-column primary key. Please use FindAll with a SingleOrDefault clause instead.");
			}

			var idExpression = Extensions.BuildIdExpression<T>(id, keyName);

			return context.GetTable<T>()
				.SingleOrDefault(idExpression);
		}

		public IQueryable<T> FindAll() {
			var context = ContextScope.Current.Context;
			return context.GetTable<T>();
		}

		public void Save(T entity) {
			var context = ContextScope.Current.Context;
			context.GetTable<T>().InsertOnSubmit(entity);
		}

		public void Delete(T entity) {
			var context = ContextScope.Current.Context;
			context.GetTable<T>().DeleteOnSubmit(entity);
		}

		private static string GetKeyName(Type contextType) {
			var keys = ActiveRecordConfiguration.Current.MappingSource.GetModel(contextType)
				.GetMetaType(typeof(T))
				.DataMembers.Where(x => x.IsPrimaryKey).ToList();

			if (keys.Count == 1) {
				return keys[0].Member.Name;
			}
			return null;
		}
	}
}