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

	/// <summary>
	/// Extension methods
	/// </summary>
	public static class Extensions {
		internal static void Guard(this object obj, string message) {
			if (obj == null) {
				throw new ArgumentNullException(message);
			}
		}

		internal static void Guard(this string str, string message) {
			if (string.IsNullOrEmpty(str)) {
				throw new ArgumentNullException(message);
			}
		}

		/// <summary>
		/// Gets the property name from a member expression.
		/// </summary>
		public static string GetPropertyName<T, TProperty>(this Expression<Func<T, TProperty>> expression) {
			var memberExp = RemoveUnary(expression.Body);

			if (memberExp == null) {
				throw new InvalidOperationException(string.Format("Expected a member expression for expression: {0}", expression));
			}

			return memberExp.Member.Name;
		}

		public static Type ReturnType(this MemberInfo member) {
			if (member is PropertyInfo) {
				return ((PropertyInfo)member).PropertyType;
			}

			if (member is FieldInfo) {
				return ((FieldInfo)member).FieldType;
			}

			return null;
		}

		public static MemberInfo GetMember<T, TMember>(this Expression<Func<T, TMember>> expression) {
			var memberExp = RemoveUnary(expression.Body);

			if(memberExp == null) {
				return null;
			}

			return memberExp.Member;
		}

		public static object GetValue(this MemberInfo member, object obj) {
			if(member is PropertyInfo) {
				return ((PropertyInfo)member).GetValue(obj, null);
			}

			if(member is FieldInfo) {
				return ((FieldInfo)member).GetValue(obj);
			}

			return null;
		}

		public static void SetValue(this MemberInfo member, object obj, object value) {
			if (member is PropertyInfo) {
				((PropertyInfo)member).SetValue(obj, value, null);
			}
			else if (member is FieldInfo) {
				((FieldInfo)member).SetValue(obj, value);
			}
		}

		/// <summary>
		/// Gets the PropertyInfo from a member expression
		/// </summary>
		public static PropertyInfo GetProperty<T, TProperty>(this Expression<Func<T, TProperty>> expression) {
			var memberExp = RemoveUnary(expression.Body);

			if (memberExp == null) {
				return null;
			}

			return memberExp.Member as PropertyInfo;
		}

		/// <summary>
		/// Gets the MethodInfo from a Lambda expression.
		/// </summary>
		public static MethodInfo GetMethod(this LambdaExpression expression) {
			var methodCall = expression.Body as MethodCallExpression;

			if(methodCall == null) {
				return null;
			}

			return methodCall.Method;
		}

		private static MemberExpression RemoveUnary(Expression toUnwrap) {
			if (toUnwrap is UnaryExpression) {
				return (MemberExpression)((UnaryExpression)toUnwrap).Operand;
			}

			return toUnwrap as MemberExpression;
		}

		internal static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
			foreach (var item in source) {
				action(item);
			}
		}


		public static Expression<Func<T, bool>> BuildIdExpression<T>(object id, string keyName) {
			var parameter = Expression.Parameter(typeof(T), "x");

			var expression = Expression.Lambda<Func<T, bool>>(
				Expression.Equal(
					Expression.Property(parameter, keyName),
					Expression.Constant(id)
				),
				parameter
			);

			return expression;
		}

	}
}