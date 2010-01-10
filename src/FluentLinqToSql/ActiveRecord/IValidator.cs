namespace FluentLinqToSql.ActiveRecord {
	public interface IValidator {
		ValidationResult Validate(object toValidate);
	}
}