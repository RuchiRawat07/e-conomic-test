using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Timelogger.Entities
{
	public class FeatureSettings
	{
        public int Id { get; set; }
		public string Identifier { get; set; }
        
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        
        [ForeignKey("User")]
        [AllowNull]
        public int UserId { get; set; }
		public Boolean IsEnabled { get; set; }
	}
}
