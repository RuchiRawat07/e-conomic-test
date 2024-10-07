using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Timelogger.Api.DataHandler;
using Timelogger.Api.Dtos;
using Timelogger.Api.Validators;


namespace Timelogger.Api.Controllers
{
	[Route("api/[controller]")]
	public class ActivityRegisterController : Controller
	{
		private readonly ApiContext _context;
        private readonly ActivityRegisterHandler _activityRegisterHandler;

        public ActivityRegisterController(ActivityRegisterHandler activityRegisterHandler, ApiContext context)
        {
            _activityRegisterHandler = activityRegisterHandler;
            _context = context;
        }

        // GET api/activityregister
        [HttpGet]
        public IActionResult Get([FromQuery] int projectId)
        {
            var filteredActivityRegisters = _activityRegisterHandler.GetActivityRegistersByProjectId(projectId);
        
            return Ok(filteredActivityRegisters);
        }

        // POST api/activityregister
        [HttpPost]
        public IActionResult Create([FromBody] CreateActivityRegisterDto createDto)
        {
            var (IsValid, ErrorResult) = ValidateCreateEntryRequest(createDto);
            if (!IsValid)
            {
                return ErrorResult; 
            }

            var activityRegister = new Entities.ActivityRegister
            {
                ActivityId = createDto.ActivityId,
                ProjectId = createDto.ProjectId,
                CreatedOn = DateTime.UtcNow,
                StartTime = createDto.StartTime,
                EndTime = createDto.EndTime
            };

            _context.ActivityRegister.Add(activityRegister);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = activityRegister.Id }, activityRegister);
        }

        public (bool IsValid, IActionResult ErrorResult) ValidateCreateEntryRequest(CreateActivityRegisterDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return (false, BadRequest(ModelState));
            }

            var createEntryValidator = new CreateActivityRegisterValidator();
            var (IsValid, ErrorResult) = createEntryValidator.ProjectValidations(createDto, _context);

            if (!IsValid)
            {
                return (IsValid, BadRequest(new {data = ErrorResult}));
            }
            return (true, null);
        }
	}
}
