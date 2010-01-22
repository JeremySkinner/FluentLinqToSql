namespace FluentLinqToSql {
	using System;
	using System.Collections.Generic;
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Linq;
	using System.Reflection;
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.Internal;
	using FluentLinqToSql.Mappings;

	public class MappingConventions {
		public Func<Type, IEnumerable<IColumnMapping>> PrimaryKeySelector = DefaultPrimaryKeySelector;
		public Func<Type, string> TableNameSelector = DefaultTableNameSelector;
		public Func<Type, IMapping> MappingCreator = DefaultMappingCreator;
		public Func<Type, IEnumerable<Type>, IEnumerable<IBelongsToMapping>> BelongsToSelector = DefaultBelongsToSelector;
		public Func<Type, IEnumerable<Type>, IEnumerable<IHasManyMapping>> HasManySelector = DefaultHasManySelector;
		public Func<Type, IEnumerable<MemberInfo>, IEnumerable<IColumnMapping>> PropertySelector = DefaultPropertySelector;


		private static IEnumerable<IHasManyMapping> DefaultHasManySelector(Type type, IEnumerable<Type> otherTypes) {
			var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

			var propsWithAttributes = from prop in props
									  let attr = (AssociationAttribute)Attribute.GetCustomAttribute(prop, typeof(AssociationAttribute))
									  where attr != null
									  where attr.IsForeignKey == false
									  select new { prop, attr };

			var propsWithoutAttributes = from prop in props
										 where Attribute.IsDefined(prop, typeof(NotMappedAttribute)) == false
										 where Attribute.IsDefined(prop, typeof(AssociationAttribute)) == false
										 let entityType = GetOneToManyType(prop.PropertyType)
										 where entityType != null
										 where otherTypes.Contains(entityType)
										 select new{ prop, attr = new AssociationAttribute() {
										                                              	OtherKey = type.Name + "Id"
										                                              } };

			var toReturn = new List<IHasManyMapping>();

			foreach (var propsToMap in propsWithAttributes.Concat(propsWithoutAttributes)) {
				var map = CreateHasManyMapping(type, propsToMap.prop.PropertyType, propsToMap.prop);
				CopyAttributeToMapping(propsToMap.attr, map);
				toReturn.Add(map);
			}

			return toReturn;

		}

		private static Type GetOneToManyType(Type type) {
			if(type.IsGenericType) {
				var genericType = type.GetGenericTypeDefinition();
				bool isValid = genericType == typeof(IList<>) || genericType == typeof(ICollection<>) || genericType == typeof(EntitySet<>);
				if(isValid) {
					return type.GetGenericArguments()[0];
				}
			}

			return null;
		}

		private static IEnumerable<IColumnMapping> DefaultPropertySelector(Type type, IEnumerable<MemberInfo> memberInfos) {
			var props = from prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
						where !memberInfos.Contains(prop)
						where !Attribute.IsDefined(prop, typeof(NotMappedAttribute))
						let attr = (ColumnAttribute)Attribute.GetCustomAttribute(prop, typeof(ColumnAttribute)) ?? new ColumnAttribute()
						select new { prop, attr };

			var toReturn = new List<IColumnMapping>();

			foreach(var prop in props) {
				var map = new ColumnMapping(prop.prop);
				CopyAttributeToMapping(prop.attr, map);
				toReturn.Add(map);
			}

			return toReturn;
		}

		private static string DefaultTableNameSelector(Type type) {
			var attr = (TableAttribute)Attribute.GetCustomAttribute(type, typeof(TableAttribute));
			if (attr != null && !string.IsNullOrEmpty(attr.Name)) {
				return attr.Name;
			}

			return type.Name;
		}

		private static IEnumerable<IBelongsToMapping> DefaultBelongsToSelector(Type type, IEnumerable<Type> otherTypes) {
			var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

			var propsWithAttributes = from prop in props
									  let attr = (AssociationAttribute)Attribute.GetCustomAttribute(prop, typeof(AssociationAttribute))
									  where attr != null
									  where attr.IsForeignKey
									  select new { prop, attr };

			var propsWithoutAttributes = from prop in props
										 where Attribute.IsDefined(prop, typeof(NotMappedAttribute)) == false
										 where Attribute.IsDefined(prop, typeof(AssociationAttribute)) == false
										 where GetOneToManyType(prop.PropertyType) == null
										 where otherTypes.Contains(prop.PropertyType)
										 select new{ prop, attr = new AssociationAttribute() {
										                                              	IsForeignKey = true,
																						ThisKey = prop.Name + "Id",
										                                              } };

			var toReturn = new List<IBelongsToMapping>();

			foreach(var propsToMap in propsWithAttributes.Concat(propsWithoutAttributes)) {
				var map = CreateBelongsToMapping(type, propsToMap.prop.PropertyType, propsToMap.prop);
				CopyAttributeToMapping(propsToMap.attr, map);
				toReturn.Add(map);
			}

			return toReturn;
		}

		private static IBelongsToMapping CreateBelongsToMapping(Type instanceType, Type propertyType, MemberInfo member) {
			return (IBelongsToMapping)Activator.CreateInstance(typeof(BelongsToMapping<,>).MakeGenericType(instanceType, propertyType), member);
		}

		private static IHasManyMapping CreateHasManyMapping(Type instanceType, Type propertyType, MemberInfo member) {
			return (IHasManyMapping)Activator.CreateInstance(typeof(HasManyMapping<,>).MakeGenericType(instanceType, propertyType), member);
		}

		private static IMapping DefaultMappingCreator(Type type) {
			var mappingType = typeof(Mapping<>).MakeGenericType(type);
			return (IMapping)Activator.CreateInstance(mappingType); 
		}

		private static IEnumerable<IColumnMapping> DefaultPrimaryKeySelector(Type type) {
			var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			var idProps = (from prop in props
						  let attr = (ColumnAttribute)Attribute.GetCustomAttribute(prop, typeof(ColumnAttribute))
						  where attr != null
						  where attr.IsPrimaryKey
						  select new { prop, attr }).ToList();

			if(idProps.Count == 0) {
				var idProperty = type.GetProperty("Id", BindingFlags.Instance | BindingFlags.Public);

				if (idProperty == null || !idProperty.CanWrite || !idProperty.CanRead) {
					throw new Exception(string.Format("Could not find a public read/write property named Id on type {0}", type));
				}

				idProps.Add(new { 
					prop = idProperty, 
					attr = new ColumnAttribute() { IsPrimaryKey = true, IsDbGenerated = true } 
				});
			}

			var toReturn = new List<IColumnMapping>();

			foreach(var prop in idProps) {
				var map = new ColumnMapping(prop.prop);
				CopyAttributeToMapping(prop.attr, map);
				toReturn.Add(map);
			}

			return toReturn;
		}

		private static void CopyAttributeToMapping(ColumnAttribute attribute, IColumnMapping mapping) {
			if (attribute == null) return;

			if (attribute.IsDbGenerated) mapping.DbGenerated();
			if (attribute.IsPrimaryKey) mapping.PrimaryKey();
			if (attribute.IsVersion) mapping.Version();
			if (!attribute.CanBeNull) mapping.NotNull();

			if (!string.IsNullOrEmpty(attribute.DbType)) mapping.DbType(attribute.DbType);
			if (!string.IsNullOrEmpty(attribute.Name)) mapping.Named(attribute.Name);
			if (!string.IsNullOrEmpty(attribute.Expression)) mapping.Expression(attribute.Expression);
			if (!string.IsNullOrEmpty(attribute.Storage)) mapping.Storage(attribute.Storage);


			mapping.AutoSync(attribute.AutoSync);
			mapping.UpdateCheck(attribute.UpdateCheck);

			//TODO: Discriminator
		}

		private static void CopyAttributeToMapping(AssociationAttribute attr, IPropertyMapping belongsTo) {
			if (attr.IsForeignKey) belongsTo.Attributes[Constants.IsForeignKey] = "true";
			if (!string.IsNullOrEmpty(attr.ThisKey)) belongsTo.Attributes[Constants.ThisKey] = attr.ThisKey;
			if (!string.IsNullOrEmpty(attr.OtherKey)) belongsTo.Attributes[Constants.OtherKey] = attr.OtherKey;
			if(!string.IsNullOrEmpty(attr.DeleteRule)) belongsTo.Attributes[Constants.DeleteRule] = attr.DeleteRule;
			if(!string.IsNullOrEmpty(attr.Storage)) belongsTo.Attributes[Constants.Storage] = attr.Storage;
		}
	}

}