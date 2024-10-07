using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Timelogger.Api.Dtos;


namespace Timelogger.Api.DataHandler
{
    public class ActivityRegisterHandler
    {
        private readonly ApiContext _context;

        public ActivityRegisterHandler(ApiContext context)
        {
            _context = context;
        }

        public List<ActivityRegisterDto> GetActivityRegistersByProjectId(int projectId)
        {
            // TODO: Filter ActivityRegister by user

            var filteredActivityRegisters = _context.ActivityRegister
                .Where(ar => ar.ProjectId == projectId)
                .Include(ar => ar.Project)
                .Include(ar => ar.Activity)
                .Select(ar => new ActivityRegisterDto
                {
                    Id = ar.Id,
                    StartTime = ar.StartTime,
                    EndTime = ar.EndTime,
                    IsBillable = ar.Activity.IsBillable,
                    ProjectName = ar.Project.Name,
                    ActivityName = ar.Activity.Name
                })
                .ToList();

            return filteredActivityRegisters;
        }
    }
}
