namespace FluentLinqToSql.ActiveRecord {
	public class ValidationError {
		public string ErrorMessage { get; private set; }
		public string Name { get; private set; }
		public object Value { get; private set; }

		public ValidationError(string errorMessage, string name, object value) {
			ErrorMessage = errorMessage;
			Name = name;
			Value = value;
		}
	}
}