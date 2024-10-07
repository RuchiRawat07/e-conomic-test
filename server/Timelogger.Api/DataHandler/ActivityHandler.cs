using System.Collections.Generic;
using System.Linq;


namespace Timelogger.Api.DataHandler
{
    public class ActivityHandler
    {
        private readonly ApiContext _context;

        public ActivityHandler(ApiContext context)
        {
            _context = context;
        }

        public List<ActivitiesDto> GetActivities()
        {
            var activityDtos = _context.Activity
				.Select(a => new ActivitiesDto
				{
					Id = a.Id,
					ActivityName = a.Name 
				})
				.ToList();

            return activityDtos;
        }
    }
}
