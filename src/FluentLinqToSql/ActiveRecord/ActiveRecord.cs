namespace FluentLinqToSql.ActiveRecord
{
	using System;
	using System.Linq;
	using FluentLinqToSql.Internal;

	public interface IActiveRecord { }

	public class ActiveRecord<TEntity> : IActiveRecord where TEntity : ActiveRecord<TEntity>
	{
		public static TEntity FindById(object id) {
			var context = ContextScope.Current.Context;
			var keyName = GetKeyName(context.GetType());

			if(string.IsNullOrEmpty(keyName)) {
				throw new NotSupportedException("FindById is not supported for entities that do not have a single-column primary key. Please use FindAll with a SingleOrDefault clause instead.");
			}

			var idExpression = Extensions.BuildIdExpression<TEntity>(id, keyName);

			return context.GetTable<TEntity>()
				.SingleOrDefault(idExpression);
		}

		public static IQueryable<TEntity> FindAll() {
			var context = ContextScope.Current.Context;
			return context.GetTable<TEntity>();
		}

		public void Save() {
			var context = ContextScope.Current.Context;
			context.GetTable<TEntity>().InsertOnSubmit((TEntity) this);
		}

		public void Delete() {
			var context = ContextScope.Current.Context;
			context.GetTable<TEntity>().DeleteOnSubmit((TEntity) this);
		}

		private static string GetKeyName(Type contextType) {
			var keys = ActiveRecordConfiguration.Current.MappingSource.GetModel(contextType)
				.GetMetaType(typeof(TEntity))
				.DataMembers.Where(x => x.IsPrimaryKey).ToList();

			if(keys.Count == 1) {
				return keys[0].Member.Name;
			}
			return null;
		}
	}
}