namespace FluentLinqToSql.ActiveRecord {
	using System;

	public class AcitveRecordException : Exception {
		public AcitveRecordException(string msg) : base(msg) {
			
		}
	}

	public class InvalidConfigurationException : AcitveRecordException {
		public InvalidConfigurationException(string msg) : base(msg) {
		}
	}
}