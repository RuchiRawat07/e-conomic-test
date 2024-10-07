using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Timelogger.Api.DataHandler;

namespace Timelogger.Api.Controllers
{
	[Route("api/[controller]")]
	public class ActivitiesController : Controller
	{
		private readonly ApiContext _context;

		private readonly ActivityHandler _activityHandler;

		public ActivitiesController(ActivityHandler activityHandler, ApiContext context)
		{
			_activityHandler = activityHandler;
			_context = context;
		}

		// GET api/activities
		[HttpGet]
		public IActionResult Get()
		{
			var activityDtos = _activityHandler.GetActivities();

			return Ok(activityDtos);
		}

	}
}
