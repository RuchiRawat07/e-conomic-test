using Microsoft.AspNetCore.Mvc;
using Timelogger.Api.DataHandler;

namespace Timelogger.Api.Controllers
{
	[Route("api/[controller]")]
	public class ProjectsController : Controller
	{
		private readonly ApiContext _context;

		private readonly ProjectHandler _projectHandler;

		public ProjectsController(ProjectHandler projectHandler, ApiContext context)
		{
			_projectHandler = projectHandler;
			_context = context;
		}
		// GET api/projects
		[HttpGet]
		public IActionResult Get()
		{
			var projectDtos = _projectHandler.GetProjects();

			return Ok(projectDtos);
		}

	}
}
