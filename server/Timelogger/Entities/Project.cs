using System;

namespace Timelogger.Entities
{
	public class Project
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime StartDate {get; set;}
		public DateTime? CompletionDate {get; set;}
		public DateTime DeadLine {get; set;}
		public Decimal ChargesPerMinute {get; set;}
		public string Currency { get; set; }

		// Can have a many-to-many relationship with user
	}
}
