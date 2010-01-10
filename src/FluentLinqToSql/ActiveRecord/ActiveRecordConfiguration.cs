namespace FluentLinqToSql.ActiveRecord {
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Reflection;
	using FluentLinqToSql.ActiveRecord.Conventions;

	public class ActiveRecordConfiguration : IActiveRecordConfiguration {
		static ActiveRecordConfiguration current;

		public static ActiveRecordConfiguration Current {
			get {
				if (current == null) {
					throw new InvalidConfigurationException("ActiveRecord has not been configured. Please call ActiveRecordConfiguration.Configure in your application startup routine.");
				}
				return current;
			}
		}

		protected ActiveRecordConfiguration() {
			DataContextFactory = (connString, mappingSource) => new DataContext(connString, mappingSource);
			ScopeStorage = new DefaultScopeStorage();
			mappingConventions = new MappingConventions();
			ValidatorFactory = type => new DataAnnotationsValidator(type);
		}

		public static void Configure(Action<IActiveRecordConfiguration> configurator) {
			if (current != null) {
				throw new Exception("ActiveRecord has already been configured.");
			}
			current = new ActiveRecordConfiguration();
			configurator(Current);
			Current.MappingSource = Current.mappingSourceBuilder();
			Current.AssertValid();
		}

		public void AssertValid() {
			if (ConnectionString == null) {
				throw new InvalidConfigurationException("No connection string has been specified.");
			}

			if (DataContextFactory == null) {
				throw new InvalidConfigurationException("No DataContextFactory has been specified");
			}

			if (ScopeStorage == null) {
				throw new InvalidConfigurationException("No ScopeStorage specified.");
			}

			if (MappingSource == null) {
				throw new InvalidConfigurationException("The mapping source has not been initialized. Ensure that you have called MapTypes, MapTypesFromAssembly or MapTypesFromAssemblyContaining and you have classes that inherit from ActiveRecord<T>.");
			}
		}

		public static void Reset() {
			current = null;
		}

		public string ConnectionString { get; private set; }
		public DataContextFactory DataContextFactory { get; private set; }
		public ValidatorFactory ValidatorFactory { get; private set; }

		internal IScopeStorage ScopeStorage { get; private set; }

		private Func<MappingSource> mappingSourceBuilder = () => null;

		public MappingSource MappingSource { get; private set; }
		private MappingConventions mappingConventions;

		void IActiveRecordConfiguration.ConnectionStringIs(string connectionString) {
			ConnectionString = connectionString;
		}

		void IActiveRecordConfiguration.ConnectionStringFromConfigFile(string connectionStringName) {
			ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
		}

		void IActiveRecordConfiguration.DataContextFactory(DataContextFactory factory) {
			DataContextFactory = factory;
		}

		void IActiveRecordConfiguration.ScopeStorage(IScopeStorage storage) {
			ScopeStorage = storage;
		}

		void IActiveRecordConfiguration.MapTypes(IEnumerable<Type> types) {
			mappingSourceBuilder = () => {
				return new AutoMappingSource(x => true, mappingConventions)
					.AddTypes(types);
			};
		}

		void IActiveRecordConfiguration.MapTypes(params Type[] types) {
			((IActiveRecordConfiguration)this).MapTypes((IEnumerable<Type>)types);
		}

		void IActiveRecordConfiguration.MapTypesFromAssembly(Assembly assembly) {
			mappingSourceBuilder = () => {
				Predicate<Type> predicate = x => typeof(IActiveRecord).IsAssignableFrom(x) && !x.IsAbstract;

				return new AutoMappingSource(predicate, mappingConventions)
					.AddFromAssembly(assembly);
			};
		}

		public void MapTypesFromAssemblyContaining<T>() {
			((IActiveRecordConfiguration)this).MapTypesFromAssembly(typeof(T).Assembly);
		}

		void IActiveRecordConfiguration.MappingSource(MappingSource source) {
			mappingSourceBuilder = () => source;
		}

		void IActiveRecordConfiguration.ConnectToSqlServer(string server, string database) {
			ConnectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=SSPI;", server, database);
		}

		void IActiveRecordConfiguration.ConnectToSqlServer(string server, string database, string user, string password) {
			ConnectionString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};", server, database, user, password);
		}

		void IActiveRecordConfiguration.ValidatorFactory(ValidatorFactory factory) {
			ValidatorFactory = factory;
		}

		internal DataContext CreateContext(Type contextType) {
			if(contextType != typeof(DataContext)) {
				throw new NotImplementedException("Multiple context types aren't currently supported");
			}
			return DataContextFactory(ConnectionString, MappingSource);
		}

		public MappingConventions Conventions {
			get { return mappingConventions; }
		}
	}

	public delegate DataContext DataContextFactory(string connectionString, MappingSource mappingSource);
	public delegate IValidator ValidatorFactory(Type typeToValidate);

	public interface IActiveRecordConfiguration {
		void ConnectionStringIs(string connectionString);
		void ConnectionStringFromConfigFile(string connectionStringName);
		void DataContextFactory(DataContextFactory factory);
		void ScopeStorage(IScopeStorage storage);
		void MapTypes(IEnumerable<Type> types);
		void MapTypes(params Type[] types);
		void MapTypesFromAssembly(Assembly assembly);
		void MapTypesFromAssemblyContaining<T>();
		void MappingSource(MappingSource source);
		void ConnectToSqlServer(string server, string database);
		void ConnectToSqlServer(string server, string database, string user, string password);
		void ValidatorFactory(ValidatorFactory factory);

		MappingConventions Conventions { get; }
	}
}