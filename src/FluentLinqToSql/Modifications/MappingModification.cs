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
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Mappings;

	/// <summary>
	/// Base class for mapping modifications.
	/// </summary>
	public class MappingModification : IMappingModification {
		/// <summary>
		/// Applies the modification to a particular mapping.
		/// </summary>
		/// <param name="mapping">The mapping to which the modification should be applied</param>
		public void ApplyTo(IMapping mapping) {
			HandleMapping(mapping);
			HandleColumnMappings(mapping.Mappings.OfType<IColumnMapping>());
			HandleHasManyMappings(mapping.Mappings.OfType<IHasManyMapping>());
			HandleHasOneMappings(mapping.Mappings.OfType<IHasOneMapping>());
			HandleBelongsToMappings(mapping.Mappings.OfType<IBelongsToMapping>());
		}

		/// <summary>
		/// Applies the modification to many-to-one mappings.
		/// </summary>
		/// <param name="mappings">The mappings to which the modification should be applied</param>
		protected virtual void HandleBelongsToMappings(IEnumerable<IBelongsToMapping> mappings) { }

		/// <summary>
		/// Applies the modification to one-to-one mappings. 
		/// </summary>
		/// <param name="mappings">The mappings to which the modification should be applied.</param>
		protected virtual void HandleHasOneMappings(IEnumerable<IHasOneMapping> mappings) {}

		/// <summary>
		/// Applies the modification to one-to-many mappings.
		/// </summary>
		/// <param name="mappings">The one-to-many mappings to which the modification should be applied.</param>
		protected virtual void HandleHasManyMappings(IEnumerable<IHasManyMapping> mappings) {}

		/// <summary>
		/// Applies the modification to column mappings.
		/// </summary>
		/// <param name="mappings">The column mappings to which the modification should be applied.</param>
		protected virtual void HandleColumnMappings(IEnumerable<IColumnMapping> mappings) {}

		/// <summary>
		/// Applies the modification to the mapping.
		/// </summary>
		/// <param name="mapping">The mapping to which the modification should be applied.</param>
		protected virtual void HandleMapping(IMapping mapping) {}
	}
}