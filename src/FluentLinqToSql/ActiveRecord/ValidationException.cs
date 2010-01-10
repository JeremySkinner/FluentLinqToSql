namespace FluentLinqToSql.ActiveRecord {
	using System;
	using System.Collections;
	using System.Collections.Generic;

	public class ValidationException : Exception, IEnumerable<ValidationError> {
		readonly IEnumerable<ValidationError> failures;

		public ValidationException(Type type, IEnumerable<ValidationError> failures)
			: base(string.Format("Validation failed for type {0}.", type)) {
			this.failures = failures;
		}

		public IEnumerator<ValidationError> GetEnumerator() {
			return failures.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}