using System;
using Timelogger.Api.Dtos;

namespace Timelogger.Api.Services
{
    public class DurationValidationService : IBaseValidationService
    { 
        public string Identifier { get => "durationRule"; }

        // Implementing the validate method from IBaseValidationService
        public ValidationResultDto Validate(int projectId, int activityId, DateTime startTime, DateTime endTime)
        {
            var result = new ValidationResultDto();

                if (startTime >= endTime)
                {
                    result.IsValid = false;
                    result.Errors.Add("Start time must be earlier than End time.");
                }
                else
                {
                    // Calculate the time difference in minutes
                    TimeSpan timeDifference = endTime - startTime;
                    double minutesDifference = timeDifference.TotalMinutes;

                    // Check if the difference is at least 30 minutes
                    if (minutesDifference >= 30)
                    {
                        result.IsValid = true; 
                    }
                    else
                    {
                        result.IsValid = false;
                        result.Errors.Add("The time difference must be at least 30 minutes.");
                    }
                }
            
            // Return the validation result
            return result;
        }
    }
}
