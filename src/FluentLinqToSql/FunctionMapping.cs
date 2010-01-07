namespace FluentLinqToSql {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Xml.Linq;
	using Internal;
	using Mappings;

	public class FunctionMapping<T> : IMapping {

		private readonly List<IElementMapping> functionMappings = new List<IElementMapping>();
		private readonly Dictionary<string, object> customProperties = new Dictionary<string, object>();

		public Type MappedType {
			get { return typeof(T); }
		}

		public IEnumerable<XElement> ToXml() {
			return functionMappings.Select(mapping => mapping.ToXml().Single());
		}

		public IList<IElementMapping> Mappings {
			get { return functionMappings; }
		}

		public IDictionary<string, object> CustomProperties {
			get { return customProperties; }
		}

		public IFunctionMethodMapping Map(Expression<Action<T>> methodExpression) {
			methodExpression.Guard("An Expression must be specified when calling Map");

			var parser = new FunctionMappingExpressionParser(methodExpression);
			var result = parser.Process();

			var mapping = new FunctionMethodMapping(result.Method, result.Parameters);
			functionMappings.Add(mapping);

			return mapping;
		}

		[BindingMatcher]
		public static TParameter Parameter<TParameter>(Action<FunctionParameterMapping> param) {
			throw new NotImplementedException("This method should only be called as part of a binding expression for a function/stored procedure.");
		}
	}
}