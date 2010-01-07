namespace FluentLinqToSql.DatabaseTests {
	using NUnit.Framework;

	[TestFixture]
	public class AutoMappingTester : DatabaseTest {

		protected override System.Data.Linq.Mapping.MappingSource CreateMappingSource() {
			return base.CreateMappingSource();
		}

	}

	public class AutoMappingSource : FluentMappingSource {
		
	}


}