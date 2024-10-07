using System.Collections.Generic;

namespace Timelogger.Api.Dtos
{
    public class ValidationResultDto
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
