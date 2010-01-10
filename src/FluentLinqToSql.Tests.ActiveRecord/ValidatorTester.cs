namespace FluentLinqToSql.Tests.ActiveRecord {
	using System.ComponentModel.DataAnnotations;
	using System.Globalization;
	using System.Linq;
	using System.Threading;
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.Tests.ActiveRecord.Model;
	using NUnit.Framework;

	[TestFixture]
	public class ValidatorTester {
		[Test]
		public void ValidationFailsForInvalidValue() {
			var validator = new DataAnnotationsValidator(typeof(Foo1));
			var foo = new Foo1();
			var result = validator.Validate(foo);

			result.IsValid.ShouldBeFalse();
		}

		[Test]
		public void ValidationSucceedsForValidValue() {
			var validator = new DataAnnotationsValidator(typeof(Foo1));
			var foo = new Foo1() { Name = "Jeremy" };
			var result = validator.Validate(foo);

			result.IsValid.ShouldBeTrue();
		}

		[Test]
		public void FormatsErrorMessage() {
			var originalCulture = Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-gb");

			try {
				var validator = new DataAnnotationsValidator(typeof(Foo1));
				var result = validator.Validate(new Foo1());
				result.Single().ErrorMessage.ShouldEqual("The Name field is required.");
			}
			finally {
				Thread.CurrentThread.CurrentCulture = originalCulture;
			}
		}

		[Test]
		public void ValidatesAtObjectLevel() {
			var validator = new DataAnnotationsValidator(typeof(Foo2));
			var result = validator.Validate(new Foo2 { Id = 1, Name = "bar" });
			result.IsValid.ShouldBeFalse();
			result.Single().ErrorMessage.ShouldEqual("Id: 1 Name: bar");
		}

		private class Foo1 {
			[Required]
			public string Name { get; set; }
		}

		[MyValidation]
		private class Foo2 {
			public string Name { get; set; }
			public int Id { get; set; }
		}

		private class MyValidationAttribute : ValidationAttribute {

			public override bool IsValid(object value) {
				var foo = (Foo2)value;
				ErrorMessage = string.Format("Id: {0} Name: {1}", foo.Id, foo.Name);
				return false;
			}
		}
	}
}