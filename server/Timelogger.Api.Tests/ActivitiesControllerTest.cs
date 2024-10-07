using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Timelogger;
using Timelogger.Api.Controllers;
using Timelogger.Entities;
using Timelogger.Api.DataHandler;


[TestFixture]
public class ActivitiesControllerTests
{
    private ApiContext _context;
    private ActivityHandler _activityHandler;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApiContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApiContext(options);
        _activityHandler = new ActivityHandler(_context);

        _context.Activity.Add(new Activity { Id = 1, Name = "Frontend" });
        _context.Activity.Add(new Activity { Id = 2, Name = "Backend" });
        _context.Activity.Add(new Activity { Id = 3, Name = "Testing" });
        _context.SaveChanges();
    }

    [Test]
    public void Get_ReturnsAllActivities()
    {
        var controller = new ActivitiesController(_activityHandler, _context);

        var result = controller.Get(); 

        Assert.IsNotNull(result);

        var okResult = result as OkObjectResult; 
        Assert.IsNotNull(okResult);

        var activities = okResult.Value as IEnumerable<ActivitiesDto>;
   
        Assert.IsNotNull(activities);

        // Verify the number of activities returned
        Assert.AreEqual(3, activities.Count());
    }

   
    [TearDown]
    public void TearDown()
    {
        var options = new DbContextOptionsBuilder<ApiContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        using var context = new ApiContext(options);
        context.Database.EnsureDeleted(); 
    }
}
