using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services.Options;

public class MachineLearningModeOptionService : IMachineLearningModeOptionService
{
    public bool IsEnabled { get; set; }
}