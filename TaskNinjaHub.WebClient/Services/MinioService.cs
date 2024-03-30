namespace TaskNinjaHub.WebClient.Services;

public class MinioService(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Minio");

    private readonly string _bucketName = "task-files";

    public async Task<HttpResponseMessage> UploadFile(MultipartFormDataContent content)
    {
        var message = await _httpClient.PostAsync($"UploadFile?bucketName={_bucketName}", content);
        return message;
    }

    public async Task<HttpResponseMessage> UploadFileByName(string name)
    {
        var message = await _httpClient.PostAsJsonAsync($"UploadFile?bucketName={_bucketName}&{name}", string.Empty);
        return message;
    }

    public async Task<HttpResponseMessage> DeleteFile(string name)
    {
        var message = await _httpClient.PostAsJsonAsync($"DeleteFile?bucketName={_bucketName}&fileName={name}", string.Empty);
        return message;
    }
}