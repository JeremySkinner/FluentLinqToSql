namespace FluentLinqToSql.TestHelper {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.ActiveRecord.Conventions;
	using FluentLinqToSql.Internal;

	internal class FakeDataAccess<T> : IDataAccess<T> {
		readonly List<T> data = new List<T>();

		public FakeDataAccess(IEnumerable<T> fakes) {
			data.AddRange(fakes);
		}

		public T FindById(object id) {
			var meta = new MetaDataBuilderConvention().GetMetaData(typeof(T))
				.OfType<ColumnMetaData>();

			var keys = new PrimaryKeyBuilderConvention().GetPrimaryKeyColumns(typeof(T), meta)
				.Select(x => x.Member).ToList();

			if(keys.Count != 1) {
				throw new NotSupportedException("FindById is not supported for entities that do not have a single-column primary key. Please use FindAll with a SingleOrDefault clause instead.");
			}

			var idExpression = Extensions.BuildIdExpression<T>(id, keys[0].Name);

			return FindAll().SingleOrDefault(idExpression);
		}

		public IQueryable<T> FindAll() {
			return data.AsQueryable();
		}

		public void Save(T entity) {
			data.Add(entity);
		}

		public void Delete(T entity) {
			data.Remove(entity);
		}
	}
}