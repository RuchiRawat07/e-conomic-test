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
using Timelogger.Api.Validators;
using Timelogger.Entities;  

[TestFixture]
public class CreateActivityRegisterValidatorTest
{
    private CreateActivityRegisterValidator _validator;
    private ApiContext _context;

   [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApiContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            _context = new ApiContext(options);
            _validator = new CreateActivityRegisterValidator(); 
            
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
    public void ProjectValidations_ReturnsSuccess_WhenValidationPasses()
    {
        // Difference between the start and end time is 40 mins
        var createDto = new CreateActivityRegisterDto
        {
            ActivityId = 1,
            ProjectId = 1,
            StartTime = DateTime.UtcNow.AddMinutes(-40),
            EndTime = DateTime.UtcNow 
        };
        var (isValid, errors) = _validator.ProjectValidations(createDto, _context);

        // Validations pass as the time log is of 40 mins > 30 mins rule
        // Feature settings has a enabled duration rule for this project
        Assert.IsTrue(isValid);
        Assert.IsNull(errors);
    }

    [Test]
    public void ProjectValidations_ReturnsSuccess_WhenNoValidationChecked()
    {
        // For this project the durationRule is disabled in FeatureSettings
        // Allow creating entry even when the difference between the 
        // start and end time is less than 30 mins.
        var createDto = new CreateActivityRegisterDto
        {
            ActivityId = 1,
            ProjectId = 2,
            StartTime = DateTime.UtcNow.AddMinutes(-10), 
            EndTime = DateTime.UtcNow 
        };
        var (isValid, errors) = _validator.ProjectValidations(createDto, _context);
        // No validations are checked as isEnabled is false
        // Errors should be null
        Assert.IsTrue(isValid); 
        Assert.IsNull(errors); 
        
    }
    
    [Test]
    public void ProjectValidations_ReturnsError_WhenValidationFails()
    {
        // Entry StartTime is 10 mins before EndTime
        // For this project the durationRule is enabled in FeatureSettings
        var createDto = new CreateActivityRegisterDto
        {
            ActivityId = 1,
            ProjectId = 1,
            StartTime = DateTime.UtcNow.AddMinutes(-10), 
            EndTime = DateTime.UtcNow 
        };
        
        var (isValid, errors) = _validator.ProjectValidations(createDto, _context);

        // Validations fails as the time log is of 10 mins
        // Feature settings has a enabled duration rule for this project
        Assert.IsFalse(isValid);
        Assert.IsNotNull(errors);
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
