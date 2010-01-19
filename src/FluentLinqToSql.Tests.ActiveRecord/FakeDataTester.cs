#region License
// Copyright Jeremy Skinner (http://www.jeremyskinner.co.uk)
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
// The latest version of this file can be found at http://github.com/JeremySkinner/FluentLinqToSql
#endregion

namespace FluentLinqToSql.Tests.ActiveRecord {
	using System.Linq;
	using FluentLinqToSql.ActiveRecord;
	using FluentLinqToSql.TestHelper;
	using FluentLinqToSql.Tests.ActiveRecord.Model;
	using NUnit.Framework;

	[TestFixture]
	public class FakeDataTester {
		[SetUp]
		public void Setup() {
			ActiveRecordConfiguration.Configure(x => {
				x.ConnectionStringIs("foo");
//				x.MapTypes(typeof(Customer));
			});
		}

		[TearDown]
		public void Teardown() {
			ActiveRecordConfiguration.Reset();
		}

		[Test]
		public void StoresFakeInstance() {
			using (Fake.Data()) {
				var customer = new Customer {Name = "Jeremy", Id = 4};
				customer.Save();

				var customer2 = Customer.FindById(4);
				customer2.ShouldBeTheSameAs(customer);
			}
		}

		[Test]
		public void RetrievesFakesInstances() {
			var cust = new Customer {Id = 4};

			using (Fake.Data(cust)) {
				Customer.FindById(4).ShouldBeTheSameAs(cust);
			}
		}

		[Test]
		public void RetrievesAllFakeInstances() {
			var custs = new[] {
			                  	new Customer {Id = 1},
			                  	new Customer {Id = 2}
			                  };

			using(Fake.Data(custs)) {
				var all = Customer.FindAll();
				all.Count().ShouldEqual(2);
			}
		}

		[Test]
		public void DeletesInstance() {
			var cust = new Customer { Id = 4 };

			using (Fake.Data(cust)) {
				cust.Delete();
				Customer.FindAll().Count().ShouldEqual(0);
			}
		}

		[Test]
		public void ResetsDataAccess() {
			var cust = new Customer { Id = 4 };

			using (Fake.Data(cust)) { }

			Customer.dataAccess.ShouldBe<DefaultDataAccess>();

		}
	}
}