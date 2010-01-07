namespace FluentLinqToSql.Tests {
	using Mappings;
	using Modifications;
	using NUnit.Framework;

	[TestFixture]
	public class Demo {

		[Test]
		public void METHOD() {
			var mod = new TblPrefixModification();
			var map = new MyMap();
			mod.ApplyTo(map);
			map.CastTo<IMapping>().CustomProperties["TableName"].ShouldEqual("tbl_Customer");

		}

		public class TblPrefixModification  : MappingModification {
			protected override void HandleMapping(IMapping mapping) {
				string name = mapping.CustomProperties["TableName"].ToString();
				name = "tbl_" + name;
				mapping.CustomProperties["TableName"] = name;
			}
		}

		public class MyMap : Mapping<Customer> {
			public MyMap() {
				Map(x => x.Id);
			}
		}
	}
}