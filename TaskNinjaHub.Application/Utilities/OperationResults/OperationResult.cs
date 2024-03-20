namespace TaskNinjaHub.Application.Utilities.OperationResults;

public class OperationResult<T>
{
    public T Body { get; set; }

    public bool Success { get; set; }

    public string ErrorMessage { get; set; } = null!;

    public static OperationResult<T> SuccessResult()
    {
        return new OperationResult<T> { Success = true };
    }

    public static OperationResult<T> SuccessResult(T body)
    {
        return new OperationResult<T> { Success = true, Body = body };
    }

    public static OperationResult<T> FailedResult(string errorMessage)
    {
        return new OperationResult<T> { Success = false, ErrorMessage = errorMessage };
    }
}