using System;
using System.ComponentModel.DataAnnotations;


namespace Timelogger.Api.Dtos
{
public class CreateActivityRegisterDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ActivityId must be present in the request")]
        public int ActivityId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ProjectId must be present in the request")]
        public int ProjectId { get; set; }
        
        [Required]
        public DateTime StartTime { get; set; }
        
        [Required]
        public DateTime EndTime { get; set; }
    }
}