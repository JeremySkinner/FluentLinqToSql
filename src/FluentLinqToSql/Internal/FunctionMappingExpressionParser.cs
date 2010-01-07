#region License
// Copyright 2008 Jeremy Skinner (http://www.jeremyskinner.co.uk)
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://www.codeplex.com/FluentLinqToSql
#endregion

namespace FluentLinqToSql.Internal {
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Reflection;
	using Mappings;

	/// <summary>
	/// Used to parse expressions when building FunctionMappings.
	/// </summary>
	public class FunctionMappingExpressionParser {
		private readonly MethodCallExpression expression;
		private readonly List<FunctionParameterMapping> parameters = new List<FunctionParameterMapping>();

		public FunctionMappingExpressionParser(LambdaExpression expression) {
			var methodCall = expression.Body as MethodCallExpression;

			if (methodCall == null) {
				throw new ArgumentException("Expression must be a MethodCallExpression");
			}

			this.expression = methodCall;
		}

		public class Result {
			public Result(MethodInfo method, FunctionParameterMapping[] parameters) {
				Method = method;
				Parameters = parameters;
			}

			public MethodInfo Method { get; private set; }
			public FunctionParameterMapping[] Parameters { get; private set; }
		}

		public Result Process() {
			int argumentNumber = 0;
			foreach (var argument in expression.Arguments) {
				if (argument.NodeType == ExpressionType.Call) {
					HandleMethodCall((MethodCallExpression)argument, argumentNumber);
				}
				else {
					HandleStandardParameter(argumentNumber);
				}

				argumentNumber++;
			}

			return new Result(expression.Method, parameters.ToArray());
		}

		private void HandleMethodCall(MethodCallExpression methodExpression, int argumentNumber) {
			if (! IsBindingMethod(methodExpression.Method)) {
				HandleStandardParameter(argumentNumber);
				return;
			}

			var action = (Action<FunctionParameterMapping>)((LambdaExpression)methodExpression.Arguments[0]).Compile();
			var mapping = new FunctionParameterMapping(ParameterAtPosition(argumentNumber));
			parameters.Add(mapping);
			action(mapping);
			return;
		}

		private void HandleStandardParameter(int argumentNumber) {
			parameters.Add(new FunctionParameterMapping(ParameterAtPosition(argumentNumber)));
		}

		private ParameterInfo ParameterAtPosition(int index) {
			return expression.Method.GetParameters()[index];
		}

		private bool IsBindingMethod(MethodInfo method) {
			return method.GetCustomAttributes(typeof(BindingMatcherAttribute), false).Length == 1;
		}
	}
}