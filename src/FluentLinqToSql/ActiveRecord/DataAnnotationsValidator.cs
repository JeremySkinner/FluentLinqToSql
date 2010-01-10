namespace FluentLinqToSql.ActiveRecord {
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Reflection;

	public class DataAnnotationsValidator : IValidator {
		private readonly Type type;

		public DataAnnotationsValidator(Type type) {
			this.type = type;
		}

		public ValidationResult Validate(object toValidate) {
			var properties = GetProperties();

			var propsWithValidators = from property in properties
			                          let validators = GetValidators(property)
			                          where validators.Length > 0
			                          let value = GetPropertyValue(property, toValidate)
			                          select new { name = property.Name, validators, value };

			var propertyErrors = from propWithValidator in propsWithValidators
			                     from validator in propWithValidator.validators
			                     where !validator.IsValid(propWithValidator.value)
			                     let error = validator.FormatErrorMessage(propWithValidator.name)
			                     select new ValidationError(error, propWithValidator.name, propWithValidator.value);

			var objectErrors = from validator in GetValidators()
			                   where !validator.IsValid(toValidate)
			                   let error = validator.FormatErrorMessage(string.Empty)
			                   select new ValidationError(error, string.Empty, toValidate);


			return new ValidationResult(propertyErrors.Concat(objectErrors));

		}

		private object GetPropertyValue(PropertyInfo property, object toValidate) {
			return property.GetValue(toValidate, null);
		}

		private ValidationAttribute[] GetValidators(PropertyInfo property) {
			return (ValidationAttribute[])property.GetCustomAttributes(typeof(ValidationAttribute), false);
		}

		private ValidationAttribute[] GetValidators() {
			return (ValidationAttribute[])type.GetCustomAttributes(typeof(ValidationAttribute), false);
		}

		private PropertyInfo[] GetProperties() {
			return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
		}
	}
}