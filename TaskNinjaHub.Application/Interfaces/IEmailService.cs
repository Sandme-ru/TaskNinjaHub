using TaskNinjaHub.Application.Entities.Tasks.Domain;

namespace TaskNinjaHub.Application.Interfaces;

public interface IEmailService
{
    Task SendCreateEmailAsync(string to, CatalogTask task);
    Task SendUpdateEmailAsync(string to, CatalogTask task);
}