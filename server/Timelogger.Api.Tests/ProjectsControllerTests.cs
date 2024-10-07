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
public class ProjectsControllerTests
{
    private ApiContext _context;
    private ProjectHandler _projectHandler;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApiContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApiContext(options);
        _projectHandler = new ProjectHandler(_context);

        _context.Projects.Add(new Project { Id = 1, Name = "Project 1" });
        _context.Projects.Add(new Project { Id = 2, Name = "Project 2" });
        _context.SaveChanges();
    }

    /* 
    Test to ensure that the ProjectsController's Get method returns all projects.
    The test verifies that the result is not null, checks the type of the result as OkObjectResult, 
    ensures the projects collection is returned, and validates the count of projects to be 2. 
    */
    [Test]
    public void Get_ReturnsAllProjects()
    {
        var controller = new ProjectsController(_projectHandler, _context);

        var result = controller.Get(); 
        Assert.IsNotNull(result);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);

        var projects = okResult.Value as IEnumerable<ProjectDto>;
        Assert.IsNotNull(projects);

        // Verify the number of projects returned
        Assert.AreEqual(2, projects.Count()); 
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
