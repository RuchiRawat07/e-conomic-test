using System.Collections.Generic;
using System.Linq;


namespace Timelogger.Api.DataHandler
{
    public class ProjectHandler
    {
        private readonly ApiContext _context;

        public ProjectHandler(ApiContext context)
        {
            _context = context;
        }

        public List<ProjectDto> GetProjects()
        {
            var projectDtos = _context.Projects
				.Select(project => new ProjectDto
				{
					Id = project.Id,
					Name = project.Name,
					StartDate = project.StartDate,
					DeadLine = project.DeadLine
				}).ToList();

            return projectDtos;
        }
    }
}
