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

namespace FluentLinqToSql.Modifications {
	using System.Collections.Generic;
	using System.Data.Linq.Mapping;
	using Internal;
	using Mappings;

	/// <summary>
	/// Mapping modification that will change the 'UpdateCheck' property to 'Never' for all column mappings.
	/// </summary>
	public class ChangeUpdateCheckToNever : MappingModification {
		/// <summary>
		/// Applies the modification to column mappings. 
		/// </summary>
		/// <param name="mappings">The mappings to which the modification should be applied</param>
		protected override void HandleColumnMappings(IEnumerable<IColumnMapping> mappings) {
			mappings.ForEach(mapping => mapping.UpdateCheck(UpdateCheck.Never));
		}
	}
}