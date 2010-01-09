namespace FluentLinqToSql.Modifications {
	using System;
	using Internal;
	using Mappings;

	/// <summary>
	/// Modification that will set the 'storage' attribute for association mappings.
	/// </summary>
	/// <remarks>The modification will not be applied to mappings that have a Storge attribute explicitly specified.</remarks>
	public class UseFieldForAssociationStorage : MappingModification {

		/// <summary>
		/// Delegate that, when invoked, will return the name that should be used for the storage. 
		/// </summary>
		public Func<IPropertyMapping, string> NameSpecifier { get; private set; }

		/// <summary>
		/// Creates a new instance of the USeFieldForAssociationStorage class.
		/// </summary>
		/// <param name="nameSpecifier">Delegatae that, when invoked, will return the name that should be used for the underlying storage field</param>
		public UseFieldForAssociationStorage(Func<IPropertyMapping, string> nameSpecifier) {
			NameSpecifier = nameSpecifier;
		}

		/// <summary>
		/// Applies the modification to one-to-many mappings.
		/// </summary>
		/// <param name="mappings">The one-to-many mappings to which the modification should be applied.</param>
		protected override void HandleHasManyMappings(System.Collections.Generic.IEnumerable<IHasManyMapping> mappings) {
			mappings.ForEach(ApplyStorageAttribute);
		}

		/// <summary>
		/// Applies the modification to one-to-one mappings. 
		/// </summary>
		/// <param name="mappings">The mappings to which the modification should be applied.</param>
		protected override void HandleHasOneMappings(System.Collections.Generic.IEnumerable<IHasOneMapping> mappings) {
			mappings.ForEach(ApplyStorageAttribute);
		}

		/// <summary>
		/// Applies the modification to many-to-one mappings.
		/// </summary>
		/// <param name="mappings">The mappings to which the modification should be applied.</param>
		protected override void HandleBelongsToMappings(System.Collections.Generic.IEnumerable<IBelongsToMapping> mappings) {
			mappings.ForEach(ApplyStorageAttribute);
		}

		private void ApplyStorageAttribute(IPropertyMapping mapping) {
			if(! mapping.Attributes.ContainsKey(Constants.Storage)) {
				mapping.Attributes[Constants.Storage] = NameSpecifier(mapping);				
			}
		}

		/// <summary>
		/// Creates a new instance of the UseFieldForAssociationStorage class that will uses the convention that fields have a lowercase first character.
		/// </summary>
		/// <example>
		/// An association with the name 'Orders' would have a backing field named 'orders'
		/// </example>
		public static UseFieldForAssociationStorage LowercaseFirstCharacter {
			get {
				return new UseFieldForAssociationStorage(property => 
					property.Property.Name[0].ToString().ToLower() + property.Property.Name.Substring(1)
				);
			}
		}

		/// <summary>
		/// Creates a new instance of the UseFieldForAssociationStorage class that will uses the convention that fields have a lowercase first character and an underscore prefix.
		/// </summary>
		/// <example>
		/// An association with the name 'Orders' would have a backing field named '_orders'
		/// </example>
		public static UseFieldForAssociationStorage UnderscorePrefixLowercaseFirstCharacter {
			get {
				return new UseFieldForAssociationStorage(property =>
					"_" + property.Property.Name[0].ToString().ToLower() + property.Property.Name.Substring(1)
				);

			}
		}
	}
}