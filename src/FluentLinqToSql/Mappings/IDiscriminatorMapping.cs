namespace FluentLinqToSql.Mappings {
	using System;
	using System.Collections.Generic;

	public interface IDiscriminatorMapping : IElementMapping {
		IList<IMapping> SubClassMappings { get; }
	}

	public interface IDiscriminatorMapping<T, TDiscriminator> : IDiscriminatorMapping {
		IDiscriminatorMapping<T, TDiscriminator> SubClass<TSubClass>(TDiscriminator discriminatorValue, Action<SubClassMapping<TSubClass, TDiscriminator>> mapping) where TSubClass : T;
		IDiscriminatorMapping<T, TDiscriminator> BaseClassDiscriminatorValue(TDiscriminator discriminator);
	}
}