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

namespace FluentLinqToSql.Tests {
	using System;
	using NUnit.Framework;

	[TestFixture]
	public class PrivateTester {
		[Test]
		public void Should_create_expression_from_private_property() {
			var expression = Private.Property<Entity, string>("PrivateProperty");
			expression.Compile()(new Entity()).ShouldEqual("property");
		}

		[Test]
		public void Should_throw_if_property_cannot_be_found() {
			Assert.Throws<Exception>(() => Private.Property<Entity, string>("foo"));
		}

		[Test]
		public void Should_create_expression_from_private_field() {
			var expression = Private.Field<Entity, string>("privateField");
			expression.Compile()(new Entity()).ShouldEqual("field");
		}

		[Test]
		public void Should_throw_if_field_cannot_be_found() {
			Assert.Throws<Exception>(() => Private.Field<Entity, string>("foo"));
		}

		private class Entity {
			private string privateField = "field";

			private string PrivateProperty {
				get { return "property"; }
			}
		}
	}
}