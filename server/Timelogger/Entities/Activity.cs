using System;
using System.Diagnostics.CodeAnalysis;

namespace Timelogger.Entities
{
	public class Activity
	{
		public int Id { get; set; }
		public string Name { get; set; }
	    public Boolean IsBillable { get; set; }
		public DateTime CreatedOn { get; set;}

        // TODO Create a custom validation if ChargesPerMinute is added then currency is compulsory & vice versa   
        [AllowNull]
		public Decimal ChargesPerMinute {get; set;}

        [AllowNull]
		public string Currency { get; set; }

	}
}
