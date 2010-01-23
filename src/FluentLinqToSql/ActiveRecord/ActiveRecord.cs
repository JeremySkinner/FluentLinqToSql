namespace FluentLinqToSql.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Data.Linq;
	using System.Linq;
	using System.Linq.Expressions;
	using FluentLinqToSql.Internal;
	using FluentLinqToSql.TestHelper;

	public class ActiveRecord {
		internal static IDataAccess dataAccess = new DefaultDataAccess();		
	}

	public class ActiveRecord<TEntity> : ActiveRecord where TEntity : ActiveRecord<TEntity>
	{
		public static TEntity FindById(object id) {
			return dataAccess.FindById<TEntity>(id);
		}

		public static TEntity FindOne(Expression<Func<TEntity, bool>> predicate) {
			return FindAll().SingleOrDefault(predicate);
		}

		public static IQueryable<TEntity> FindAll() {
			return dataAccess.FindAll<TEntity>();
		}

		public void Save() {
			dataAccess.Save((TEntity)this);
			
		}

		public void Delete() {
			dataAccess.Delete((TEntity)this);
		}

		public virtual void Validate() {
			var validator = GetValidator();
			var result = validator.Validate(this);

			if(! result.IsValid) {
				throw new ValidationException(typeof(TEntity), result);
			}
		}

		protected virtual IValidator GetValidator() {
			return ActiveRecordConfiguration.Current.ValidatorFactory(typeof(TEntity));
		}

		protected virtual void OnValidate(ChangeAction action) {
			if(action == ChangeAction.Insert || action == ChangeAction.Update) {
				Validate();
			}
		}
	}
}