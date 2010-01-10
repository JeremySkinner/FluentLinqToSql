namespace FluentLinqToSql.ActiveRecord {
	using System.Collections.Specialized;

	public static class UpdateFromExtensions {
		public static T UpdateFrom<T>(this T item, NameValueCollection collection) where T : ActiveRecord<T> {
			var deserializer = new NameValueDeserializer();
			deserializer.Deserialize(collection, null, typeof(T), item);
			return item;
		}

		public static T UpdateFrom<T>(this T item, NameValueCollection collection, string prefix) where T: ActiveRecord<T> {
			var deserializer = new NameValueDeserializer();
			deserializer.Deserialize(collection, prefix, typeof(T), item);
			return item;
		}
	}
}