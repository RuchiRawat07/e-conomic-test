using System;
using Timelogger.Api.Dtos;

public interface IBaseValidationService{

  ValidationResultDto Validate(int projectId, int activityId, DateTime StartTime, DateTime endTime);

  // This identifer is stored in the FeatureSetting model. If the feature is enabled then while registering an
  // entry, corrosponding classe's validation method is invoked
  public string Identifier{ get;}
}