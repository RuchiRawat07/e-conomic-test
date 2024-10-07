using System;
using System.ComponentModel.DataAnnotations;


namespace Timelogger.Api.Dtos
{
public class ActivityRegisterDto
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string ActivityName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Boolean IsBillable { get; set; }

    }
}