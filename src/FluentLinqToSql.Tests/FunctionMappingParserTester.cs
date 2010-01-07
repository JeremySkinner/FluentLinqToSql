namespace FluentLinqToSql.Tests {
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using Internal;
	using NUnit.Framework;

	[TestFixture]
	public class FunctionMappingParserTester {
		public IList<Customer> GetCustomersByName(string name) { throw new NotImplementedException(); }

		[Test]
		public void Should_obtain_MethodInfo_for_simple_method() {
			Expression<Func<FunctionMappingParserTester, object>> expr = x => x.GetCustomersByName(null);
			var visitor = new FunctionMappingExpressionParser(expr);
			visitor.Process().Method.ShouldEqual(GetType().GetMethod("GetCustomersByName"));
		}

		[Test]
		public void Should_throw_when_expression_is_not_MethodCallExpression() {
			Expression<Func<string, int>> expr = s => s.Length;
			Assert.Throws<ArgumentException>(() => new FunctionMappingExpressionParser(expr));
		}

		[Test]
		public void Should_obtain_parameters_for_simple_method() {
			Expression<Func<FunctionMappingParserTester, object>> expr = x => x.GetCustomersByName(null);
			var visitor = new FunctionMappingExpressionParser(expr);
			var results = visitor.Process();

			results.Parameters.Length.ShouldEqual(1);

			var param = results.Parameters[0];
			param.Attributes["Parameter"].ShouldEqual("name");
		}

		[Test]
		public void Should_obtain_parameter_for_complex_method() {
			Expression<Func<FunctionMappingParserTester, object>> expr = 
				x => x.GetCustomersByName(
					FunctionMapping<FunctionParameterMappingTester>.Parameter<string>(param => 
						param.DbType("foo")
					)
				);

			var visitor = new FunctionMappingExpressionParser(expr);
			var results = visitor.Process();

			results.Parameters.Length.ShouldEqual(1);

			var parameter = results.Parameters[0];
			parameter.Attributes["Parameter"].ShouldEqual("name");
			parameter.Attributes["DbType"].ShouldEqual("foo");
		}
	}
}