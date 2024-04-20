using TaskNinjaHub.Application.Entities.Tasks.Domain;

namespace TaskNinjaHub.Application.Interfaces;

public interface IEmailService
{
    Task SendCreateEmailAsync(CatalogTask task);

    Task SendUpdateEmailAsync(CatalogTask task);
}