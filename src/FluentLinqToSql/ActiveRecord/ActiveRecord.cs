namespace FluentLinqToSql.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Data.Linq;
	using System.Linq;
	using FluentLinqToSql.Internal;
	using FluentLinqToSql.TestHelper;

	public interface IActiveRecord { }

	public class ActiveRecord<TEntity> : IActiveRecord where TEntity : ActiveRecord<TEntity>
	{
		internal static IDataAccess<TEntity> dataAccess = new DefaultDataAccess<TEntity>();

		public static TEntity FindById(object id) {
			return dataAccess.FindById(id);
		}

		public static IQueryable<TEntity> FindAll() {
			return dataAccess.FindAll();
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

		public static IDisposable Fake(params TEntity[] fakes) {
			return Fake((IEnumerable<TEntity>)fakes);
		}

		public static IDisposable Fake(IEnumerable<TEntity> fakes) {
			return new FakeDataScope<TEntity>(fakes);
		}
	}
}