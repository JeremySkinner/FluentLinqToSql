namespace FluentLinqToSql.Tests.ActiveRecord {
	using System.ComponentModel.DataAnnotations;
	using System.Data.Linq.Mapping;
	using System.Linq;
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.Tests.ActiveRecord.Model;
	using NUnit.Framework;
	using ValidationException = FluentLinqToSql.ActiveRecord.ValidationException;

	[TestFixture]
	public class AutoValidateTester : IntegrationTest {

		[Test]
		public void Validates_on_commit() {

			var validationException = (ValidationException)typeof(ValidationException).ShouldBeThrownBy(delegate {
				using (new ContextScope()) {
					var customer = new ValidatingCustomer();
					customer.Save();
				}
			});

			validationException.Count().ShouldEqual(1);
		}

		[Table(Name = "Customer")]
		public class ValidatingCustomer : ActiveRecord<ValidatingCustomer> {
			public int Id { get; set; }
			[Required]
			public string Name { get; set; }

			protected override void OnValidate(System.Data.Linq.ChangeAction action) {
				base.OnValidate(action);
			}
		}

		protected override void MapTypes(IActiveRecordConfiguration cfg) {
			cfg.MapTypes(typeof(ValidatingCustomer));
		}

	}
}