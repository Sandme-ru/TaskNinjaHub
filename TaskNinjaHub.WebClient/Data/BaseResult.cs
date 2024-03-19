namespace TaskNinjaHub.WebClient.Data;

public class BaseResult
{
    public bool Success { get; set; }

    public object Result { get; set; }

    public string Error { get; set; }
}