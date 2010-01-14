namespace FluentLinqToSql.TestHelper {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.Internal;

	internal class FakeDataAccess<T> : IDataAccess<T> {
		readonly List<T> data = new List<T>();

		public FakeDataAccess(IEnumerable<T> fakes) {
			data.AddRange(fakes);
		}

		public T FindById(object id) {
			//TODO: Currently only supports "Id" property
			var idExpression = Extensions.BuildIdExpression<T>(id, "Id");

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