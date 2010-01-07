namespace FluentLinqToSql.DatabaseTests {
	using NUnit.Framework;

	[TestFixture]
	public class ScalarFunctionMappingTester : DatabaseTest {

		protected override void Setup() {
			Execute("IF EXISTS (SELECT 1 from sys.objects where Object_id = OBJECT_ID('ScalarFunction')) DROP FUNCTION dbo.ScalarFunction");
			Execute(@"CREATE FUNCTION dbo.ScalarFunction() RETURNS int
AS BEGIN
RETURN 42
END
");
		}

		[Test]
		public void Should_map_scalar_function() {
			using(var context = (TestDataContext)DataContextFactory()) {
				int result = context.MeaningOfLife();
				Assert.That(result, Is.EqualTo(42));
			}
		}
	}
}