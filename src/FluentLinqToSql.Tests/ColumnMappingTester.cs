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
	using System.Data.Linq.Mapping;
	using System.Linq;
	using System.Xml.Linq;
	using Mappings;
	using NUnit.Framework;

	[TestFixture]
	public class ColumnMappingTester {
		private IColumnMapping mapping;

		[SetUp]
		public void Setup() {
			mapping = new ColumnMapping(typeof(Customer).GetProperty("Surname"));
		}

		[Test]
		public void Default_name_should_be_surname() {
			Assert.That(mapping.CastTo<IColumnMapping>().Property.Name, Is.EqualTo("Surname"));
		}

		[Test]
		public void Should_throw_when_null_is_passed_to_named() {
			Assert.Throws<ArgumentNullException>(() => mapping.Named(null));
		}

		[Test]
		public void Should_generate_correct_element_name() {
			MappingXml.ShouldBeNamed("Column");
		}

		[Test]
		public void Should_generate_name_attribute() {
			mapping.Named("foo");
			MappingXml.ShouldHaveAttribute("Name", "foo");
		}

		[Test]
		public void Should_generate_member_attribute() {
			mapping.Named("foo");
			MappingXml.ShouldHaveAttribute("Member", "Surname");
		}

		[Test]
		public void Should_have_storage_attribute() {
			mapping.Storage("_surname");
			MappingXml.ShouldHaveAttribute("Storage", "_surname");
		}

		[Test]
		public void Should_have_autosync_attribute() {
			mapping.AutoSync(AutoSync.OnInsert);
			MappingXml.ShouldHaveAttribute("AutoSync", "OnInsert");
		}

		[Test]
		public void NotNull_Should_set_canbenull_to_false() {
			mapping.NotNull();
			MappingXml.ShouldHaveAttribute("CanBeNull", "false");
		}

		[Test]
		public void Should_have_dbtype_attribute() {
			mapping.DbType("foo");
			MappingXml.ShouldHaveAttribute("DbType", "foo");
		}

		[Test]
		public void Should_have_expression_attribute() {
			mapping.Expression("Foo");
			MappingXml.ShouldHaveAttribute("Expression", "Foo");
		}

		[Test]
		public void Should_have_IsPrimaryKey_attribute() {
			mapping.PrimaryKey();
			MappingXml.ShouldHaveAttribute("IsPrimaryKey", "true");
		}

		[Test]
		public void Should_Have_version_attribute() {
			mapping.Version();
			MappingXml.ShouldHaveAttribute("IsVersion", "true");
		}

		[Test]
		public void Should_have_updatecheck_attribute() {
			mapping.UpdateCheck(UpdateCheck.Never);
			MappingXml.ShouldHaveAttribute("UpdateCheck", "Never");
		}

		[Test]
		public void Should_have_IsDbGenerated_attribute() {
			mapping.DbGenerated();
			MappingXml.ShouldHaveAttribute("IsDbGenerated", "true");
		}

		[Test]
		public void Should_store_custom_property() {
			mapping.CastTo<IColumnMapping>().CustomProperties["foo"] = "bar";
			Assert.That(mapping.CastTo<IColumnMapping>().CustomProperties["foo"], Is.EqualTo("bar"));
		}

		private XElement MappingXml {
			get { return mapping.CastTo<IColumnMapping>().ToXml().Single(); }
		}
	}
}