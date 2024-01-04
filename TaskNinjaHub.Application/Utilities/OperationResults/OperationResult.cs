namespace TaskNinjaHub.Application.Utilities.OperationResults;

public class OperationResult
{
    public bool Success { get; set; }

    public string ErrorMessage { get; set; } = null!;

    public static OperationResult SuccessResult()
    {
        return new OperationResult { Success = true };
    }

    public static OperationResult FailedResult(string errorMessage)
    {
        return new OperationResult { Success = false, ErrorMessage = errorMessage };
    }
}