
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Timelogger.Api.Dtos;


namespace Timelogger.Api.Validators
{
    public class CreateActivityRegisterValidator{
        public (bool IsValid, List<string> Errors) ProjectValidations(CreateActivityRegisterDto createDto, ApiContext _context)
        {    
            /*
            Validates rules that apply to a project. Rules that are applicable to a project are controlled by
            FeatureSetting. Any class that implements IBaseValidationService is considered as a validation rule.
            */

            var project = _context.Projects.FirstOrDefault(p => p.Id == createDto.ProjectId);
            if (createDto.StartTime < project.StartDate || (project.CompletionDate != null && createDto.EndTime > project.CompletionDate))
            {
                List<string> errorList = new List<string>
                    {
                        "Entry time falls outside the project's active period."
                    };
                return (false, errorList);
            }
            // TODO: Move this FeatureSettings linq to its own DataHandler class
            var featureSettings = _context.FeatureSettings
                                .Where(f => f.ProjectId == createDto.ProjectId & f.IsEnabled == true)
                                .ToList();
            
            Assembly assembly = Assembly.GetExecutingAssembly();

            var implementations = assembly.GetTypes()
                .Where(type => typeof(IBaseValidationService).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .ToList();

            var IsValid = true;
            var errors = new List<string>();

            foreach (var implementation in implementations)
            {
                var rule = Activator.CreateInstance(implementation);
                var identifierProperty = implementation.GetProperty("Identifier");
                var identifierValue = identifierProperty.GetValue(rule).ToString();
                foreach (var feature in featureSettings)
                    {
                        if(feature.Identifier != identifierValue ){
                            continue;
                        }
                        var validateMethod = implementation.GetMethod("Validate");
                        
                        var validationResult = validateMethod.Invoke(rule, new object[] { 
                            createDto.ProjectId, 
                            createDto.ActivityId, 
                            createDto.StartTime, 
                            createDto.EndTime });
                            
                        var result = validationResult as ValidationResultDto;

                        if (!result.IsValid)
                        {
                            IsValid = false;
                            errors = result.Errors;
                            break;
                        }
                    }

                if(!IsValid){
                    break;
                }
            }
                    
            if(!IsValid){
                return (false,errors);
            }
             return (true, null);
        }
    }
}