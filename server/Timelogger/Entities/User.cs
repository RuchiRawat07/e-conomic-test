
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Timelogger.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
        [AllowNull]

		// Many-to-Many relationship with project
        public ICollection<Project> Projects { get; set; }

        // Can have other fields like phone number, email , role etc
	}
}
