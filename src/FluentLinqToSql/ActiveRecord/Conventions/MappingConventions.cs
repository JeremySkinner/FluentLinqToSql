namespace FluentLinqToSql.ActiveRecord.Conventions {
	public class MappingConventions {
		public MappedPropertiesConvention MappedPropertiesBuilder = new MappedPropertiesConvention();
		public PrimaryKeyBuilderConvention PrimaryKeyBuilder = new PrimaryKeyBuilderConvention();
		public MappingCreationConvention MappingCreator = new MappingCreationConvention();
		public TableNameConvention TableName = new TableNameConvention();
		public MetaDataBuilderConvention MetaDataBuilder = new MetaDataBuilderConvention();
	}
}