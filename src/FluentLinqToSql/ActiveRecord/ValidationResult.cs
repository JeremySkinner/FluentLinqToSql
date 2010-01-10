namespace FluentLinqToSql.ActiveRecord {
	using System.Collections.Generic;

	public class ValidationResult : List<ValidationError> {
		public ValidationResult(IEnumerable<ValidationError> collection) : base(collection) { }
		public ValidationResult() { }

		public bool IsValid {
			get { return Count == 0; }
		}
	}
}