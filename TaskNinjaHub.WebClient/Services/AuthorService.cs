using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services;

public class AuthorService(IHttpClientFactory httpClientFactory) : BaseService<Author>(httpClientFactory)
{
    #if (DEBUG)

    protected override string BasePath => $"api/{nameof(Author).ToLower()}";

    #elif (RELEASE)

    protected override string BasePath => $"task-api/api/{nameof(Author).ToLower()}";

    #endif
}