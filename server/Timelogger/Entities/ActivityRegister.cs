using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timelogger.Entities
{
	public class ActivityRegister
	{
		public int Id { get; set; }

        [Required]
        [ForeignKey("Activity")]
        public int ActivityId { get; set; }

        [Required]
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        
        [Required]
        public DateTime StartTime { get; set; }
        
        [Required]
        public DateTime EndTime { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual Activity Activity { get; set; }

        // Navigation property to represent the many-to-one relationship with Project
        public virtual Project Project { get; set; }

	}
}
