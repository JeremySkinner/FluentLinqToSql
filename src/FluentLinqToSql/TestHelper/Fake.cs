namespace FluentLinqToSql.TestHelper {
	using System;
	using System.Collections;

	public static class Fake {
		public static IDisposable Data(params object[] data) {
			return new FakeDataScope(data);
		}

		public static IDisposable Data(IEnumerable data) {
			return new FakeDataScope(data);
		}
	}
}