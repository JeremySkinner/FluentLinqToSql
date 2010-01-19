namespace FluentLinqToSql.TestHelper {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.Internal;

	internal class FakeDataAccess : IDataAccess {
		readonly List<object> data = new List<object>();

		public FakeDataAccess(IEnumerable fakes) {
			data.AddRange(fakes.Cast<object>());
		}

		public T FindById<T>(object id) where T : class {
			//TODO: Currently only supports "Id" property
			var idExpression = Extensions.BuildIdExpression<T>(id, "Id");

			return FindAll<T>().SingleOrDefault(idExpression);
		}

		public IQueryable<T> FindAll<T>() where T : class {
			return data.OfType<T>().AsQueryable();
		}

		public void Save<T>(T entity) where T : class {
			data.Add(entity);
		}

		public void Delete<T>(T entity) where T : class {
			data.Remove(entity);
		}
	}
}