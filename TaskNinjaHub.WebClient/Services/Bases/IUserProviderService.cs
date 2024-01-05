using TaskNinjaHub.Application.Entities.Authors.Domain;

namespace TaskNinjaHub.WebClient.Services.Bases;

public interface IUserProviderService
{
    Author User { get; set; }

    Task GetUser();
}