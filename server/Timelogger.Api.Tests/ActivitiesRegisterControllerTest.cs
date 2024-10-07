using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Moq;
using Timelogger;
using Timelogger.Api.Controllers;
using Timelogger.Api.Dtos;
using Timelogger.Entities;  
using Timelogger.Api.DataHandler;

[TestFixture]
public class ActivityRegisterControllerTests
{
    private ActivityRegisterController controller;
    private ApiContext _context;
    private ActivityRegisterHandler _activityRegisterHandler;

   [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApiContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            _context = new ApiContext(options);
            _activityRegisterHandler = new ActivityRegisterHandler(_context);

            controller = new ActivityRegisterController(_activityRegisterHandler, _context); 
            
        _context.Projects.AddRange(
            new Project { Id = 1, Name = "Project 1", StartDate = new DateTime(2020, 07, 01), CompletionDate = new DateTime(2030, 07, 01) },
            new Project { Id = 2, Name = "Project 2" , StartDate = new DateTime(2023, 07, 01), CompletionDate = new DateTime(2025, 07, 01) }
        );

        _context.Activity.AddRange(
            new Activity { Id = 1, Name = "Frontend", IsBillable = true, CreatedOn = new DateTime(2024, 07, 01) },
            new Activity { Id = 2, Name = "Backend", IsBillable = true, CreatedOn = new DateTime(2024, 07, 01) },
            new Activity { Id = 3, Name = "Testing", IsBillable = false, CreatedOn = new DateTime(2024, 08, 01) }
        );

        _context.SaveChanges();

        _context.ActivityRegister.AddRange(
            new ActivityRegister 
            { 
                Id = 1, 
                ActivityId = 1, 
                ProjectId = 1, 
                StartTime = new DateTime(2024, 07, 01, 09, 00, 00), 
                EndTime = new DateTime(2024, 07, 01, 12, 00, 00),
                CreatedOn = DateTime.UtcNow 
            },
            new ActivityRegister 
            { 
                Id = 2, 
                ActivityId = 2, 
                ProjectId = 2, 
                StartTime = new DateTime(2024, 07, 02, 10, 00, 00), 
                EndTime = new DateTime(2024, 07, 02, 14, 00, 00),
                CreatedOn = DateTime.UtcNow 
            }
        );

        _context.SaveChanges();

        _context.FeatureSettings.AddRange(
            new FeatureSettings
            {
                Id = 1,
                Identifier = "durationRule",
                ProjectId = 1,
                IsEnabled = true
            },
            new FeatureSettings
            {
                Id = 3,
                Identifier = "durationRule",
                ProjectId = 2,
                IsEnabled = false
            }
        );

        _context.SaveChanges();
    }

    [Test]
    public void Get_ReturnsFilteredActivityRegisters_WhenProjectIdIsProvided()
    { 
        var result = controller.Get(1); 

        Assert.IsNotNull(result);
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        
        var resultSet = okResult.Value as IEnumerable<ActivityRegisterDto>;
        Assert.IsNotNull(resultSet);
        Console.WriteLine($"resultset Name: {resultSet}");
        
        // Only 1 ActivityRegister for ProjectId = 1
        Assert.AreEqual(1, resultSet.Count()); 
        
        var firstRegister = resultSet.First();
        Assert.AreEqual("Project 1", firstRegister.ProjectName);
        Assert.AreEqual("Frontend", firstRegister.ActivityName);
        Assert.IsTrue(firstRegister.IsBillable);
    }

    [Test]
    public void Get_ReturnsFilteredActivityRegisters_WhenProjectIdIsMissing()
    { 
        var result = controller.Get(3); 
        var okResult = result as OkObjectResult;
        var filteredActivityRegisters = okResult.Value as IEnumerable<object>;

        // Assert no ActivityRegister for ProjectId = 3
        Assert.AreEqual(0, filteredActivityRegisters.Count()); 
    }

    [Test]
    public void Create_ReturnsCreatedResponse_WhenInputIsValid()
    {
        var createDto = new CreateActivityRegisterDto
        {
            ActivityId = 1,
            ProjectId = 1,
            StartTime = DateTime.UtcNow.AddHours(-1), 
            EndTime = DateTime.UtcNow 
        };

        // Checking existing no. of entries in ActivityRegister table
        Assert.AreEqual(2, _context.ActivityRegister.Count()); 
        
        controller.Create(createDto);
        
        // There should be one new entry
        Assert.AreEqual(3, _context.ActivityRegister.Count());
    }

    [Test]
    public void ValidateCreateEntryRequest_ReturnsBadRequest_WhenActivityIsInvalid()
    {
        var createDto = new CreateActivityRegisterDto
        {
            ActivityId = 0, 
            ProjectId = 1,  
            StartTime = DateTime.UtcNow.AddHours(-1),
            EndTime = DateTime.UtcNow
        };

        // Simulate an invalid activity state
        controller.ModelState.AddModelError("ActivityId", "ActivityId is required");

        var (isValid, errorResult) = controller.ValidateCreateEntryRequest(createDto);

        Assert.IsFalse(isValid);
        Assert.IsNotNull(errorResult);
        Assert.IsInstanceOf<BadRequestObjectResult>(errorResult);
    }

    [Test]
    public void ValidateCreateEntryRequest_ReturnsBadRequest_WhenProjectIsInvalid()
    {
        var createDto = new CreateActivityRegisterDto
        {
            ActivityId = 1, 
            ProjectId = 0,  
            StartTime = DateTime.UtcNow.AddHours(-1),
            EndTime = DateTime.UtcNow
        };

        // Simulate an invalid project state
        controller.ModelState.AddModelError("ProjectId", "ProjectId is required");

        var (isValid, errorResult) = controller.ValidateCreateEntryRequest(createDto);

        Assert.IsFalse(isValid);
        Assert.IsNotNull(errorResult);
        Assert.IsInstanceOf<BadRequestObjectResult>(errorResult);
    }

    [TearDown]
    public void TearDown()
    {
        var options = new DbContextOptionsBuilder<ApiContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        using var _context = new ApiContext(options);
        _context.Database.EnsureDeleted(); 
    }
}
