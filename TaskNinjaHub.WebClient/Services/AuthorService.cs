using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services;

public class AuthorService : BaseService<Author>
{
    protected override string BasePath => nameof(Author).ToLower();

    public AuthorService(HttpClient? httpClient) : base(httpClient)
    {
    }
}