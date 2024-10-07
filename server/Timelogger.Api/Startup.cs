using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Timelogger.Entities;
using Timelogger.Api.DataHandler;

namespace Timelogger.Api
{
	public class Startup
	{
		private readonly IWebHostEnvironment _environment;
		public IConfigurationRoot Configuration { get; }

		public Startup(IWebHostEnvironment env)
		{
			_environment = env;

			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Add framework services.
			services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("e-conomic interview"));
			services.AddLogging(builder =>
			{
				builder.AddConsole();
				builder.AddDebug();
			});

			services.AddMvc(options => options.EnableEndpointRouting = false);

			if (_environment.IsDevelopment())
			{
				services.AddCors();
			}
			services.AddScoped<ProjectHandler>();
			services.AddScoped<ActivityHandler>();
			services.AddScoped<ActivityRegisterHandler>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseCors(builder => builder
					.AllowAnyMethod()
					.AllowAnyHeader()
					.SetIsOriginAllowed(origin => true)
					.AllowCredentials());
			}

			app.UseMvc();


			var serviceScopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
			using (var scope = serviceScopeFactory.CreateScope())
			{
				SeedDatabase(scope);
			}
		}

		private static void SeedDatabase(IServiceScope scope)
		{
			var context = scope.ServiceProvider.GetService<ApiContext>();
			var testProject1 = new Project
			{
				Id = 1,
				Name = "Time Tracker",
				StartDate = new DateTime(2024, 10, 07, 0, 0, 0),
				DeadLine = new DateTime(2027, 12, 30, 0, 0, 0),
				ChargesPerMinute = 1.5m,
				Currency = "DKK",
			};

			var testProject2 = new Project
			{
				Id = 2,
				Name = "IView",
				StartDate = new DateTime(2017, 10, 06, 0, 0, 0),
				DeadLine = new DateTime(2029, 07, 22, 0, 0, 0),
				ChargesPerMinute = 3m,
				Currency = "DKK",
			};

			var testProject3 = new Project
			{
				Id = 3,
				Name = "Notal Vision",
				StartDate = new DateTime(2020, 10, 20, 0, 0, 0),
				DeadLine = new DateTime(2025, 06, 15, 0, 0, 0),
				CompletionDate = new DateTime(2024, 12, 12, 0, 0, 0),
				ChargesPerMinute = 5m,
				Currency = "DKK",
			};

			var testProject4 = new Project
			{
				Id = 4,
				Name = "Certificate Portal",
				StartDate = new DateTime(2019, 10, 10, 0, 0, 0),
				DeadLine = new DateTime(2024, 12, 12, 0, 0, 0),
				ChargesPerMinute = 7m,
				Currency = "DKK",
			};

			var testUser1 = new User
			{
				Id = 1,
				Name = "Alex",
				Projects = new List<Project> { testProject1 }  
			};

			var testUser2 = new User
			{
				Id = 2,
				Name = "Amy",
				Projects = new List<Project> { testProject2,testProject3, }  
			};

			var testUser3 = new User
			{
				Id = 3,
				Name = "Jon",
				Projects = new List<Project> { testProject3 ,testProject4 }  
			};

			var testActivity1 = new Activity
			{
				Id = 1,
				Name = "Js",
				IsBillable = true,
				ChargesPerMinute = 2,
				Currency = "DKK",
				CreatedOn = new DateTime(2024, 10, 12, 0, 0, 0),
			};
			
			var testActivity2 = new Activity
			{
				Id = 2,
				Name = "Design",
				IsBillable = true,
				ChargesPerMinute = 3,
				Currency = "DKK",
				CreatedOn = new DateTime(2024, 10, 12, 0, 0, 0),
			};

			var testActivity3 = new Activity
			{
				Id = 3,
				Name = "Backend",
				IsBillable = true,
				ChargesPerMinute = 9,
				Currency = "DKK",
				CreatedOn = new DateTime(2024, 10, 13, 0, 0, 0),
			};

			var testActivity4 = new Activity
			{
				Id = 4,
				Name = "Testing",
				IsBillable = true,
				ChargesPerMinute = 2,
				Currency = "DKK",
				CreatedOn = new DateTime(2024, 10, 17, 0, 0, 0),
			};

			var testActivity5 = new Activity
			{
				Id = 5,
				Name = "Travel",
				IsBillable = false,
				ChargesPerMinute = 0,
				Currency = "",
				CreatedOn = new DateTime(2024, 10, 22, 0, 0, 0),
			};

			var testFeature = new FeatureSettings
			{
				Id = 1,
				Identifier = "durationRule",
				ProjectId = 1,
				IsEnabled = true
			};

			var testFeature1 = new FeatureSettings
			{
				Id = 2,
				Identifier = "euDirective",
				ProjectId = 1,
				IsEnabled = true
			};
			
			var testFeature2 = new FeatureSettings
			{
				Id = 3,
				Identifier = "durationRule",
				ProjectId = 2,
				IsEnabled = true
			};

			var testFeature3 = new FeatureSettings
			{
				Id = 4,
				Identifier = "durationRule",
				ProjectId = 3,
				IsEnabled = true
			};
			
			var testFeature4 = new FeatureSettings
			{
				Id = 5,
				Identifier = "durationRule",
				ProjectId = 4,
				IsEnabled = true
			};

			var testActivityRegister = new ActivityRegister
			{
				Id = 1,
				ActivityId = 1,
				ProjectId = 1,
				StartTime = new DateTime(2024, 10, 12, 10, 0, 0),
				EndTime = new DateTime(2024, 10, 12, 11, 0, 0),
				CreatedOn = new DateTime(2024, 10, 12, 0, 0, 0)
			};
			var testActivityRegister1 = new ActivityRegister
			{
				Id = 4,
				ActivityId = 4,
				ProjectId = 1,  
				StartTime = new DateTime(2024, 12, 12, 13, 0, 0),
				EndTime = new DateTime(2024, 12, 12, 16, 30, 0),
				CreatedOn = new DateTime(2024, 10, 12, 0, 0, 0)
			};
			var testActivityRegister2 = new ActivityRegister
			{
				Id = 2,
				ActivityId = 3,
				ProjectId = 3,  
				StartTime = new DateTime(2020, 09, 10, 12, 0, 0),
				EndTime = new DateTime(2020, 09, 10, 12, 30, 0),
				CreatedOn = new DateTime(2024, 10, 12, 0, 0, 0)
			};
			var testActivityRegister3 = new ActivityRegister
			{
				Id = 3,
				ActivityId = 4,
				ProjectId = 2,  
				StartTime = new DateTime(2018, 09, 10, 13, 0, 0),
				EndTime = new DateTime(2018, 09, 10, 13, 30, 0),
				CreatedOn = new DateTime(2024, 10, 12, 0, 0, 0)
			};
			var testActivityRegister4 = new ActivityRegister
			{
				Id = 5,
				ActivityId = 5,
				ProjectId = 2,  
				StartTime = new DateTime(2018, 10, 10, 13, 0, 0),
				EndTime = new DateTime(2018, 10, 10, 09, 30, 0),
				CreatedOn = new DateTime(2024, 10, 12, 0, 0, 0)
			};
			context.Projects.Add(testProject1);
			context.Projects.Add(testProject2);
			context.Projects.Add(testProject3);
			context.Projects.Add(testProject4);
			context.User.Add(testUser1);
			context.User.Add(testUser2);
			context.User.Add(testUser3);
			context.Activity.Add(testActivity1);
			context.Activity.Add(testActivity2);
			context.Activity.Add(testActivity3);
			context.Activity.Add(testActivity4);
			context.Activity.Add(testActivity5);
			context.FeatureSettings.Add(testFeature);
			context.FeatureSettings.Add(testFeature1);
			context.FeatureSettings.Add(testFeature2);
			context.FeatureSettings.Add(testFeature3);
			context.FeatureSettings.Add(testFeature4);
			context.ActivityRegister.Add(testActivityRegister);
			context.ActivityRegister.Add(testActivityRegister1);
			context.ActivityRegister.Add(testActivityRegister2);
			context.ActivityRegister.Add(testActivityRegister3);
			context.ActivityRegister.Add(testActivityRegister4);
			context.SaveChanges();
		}
	}
}