using TaskNinjaHub.Application.Entities.Bases.Interfaces;

namespace TaskNinjaHub.Application.Entities.InformationSystems.Interfaces;

/// <summary>
/// Interface IInformationSystemRepository
/// Extends the <see cref="TaskNinjaHub.Application.Entities.Bases.Interfaces.IBaseRepository{TaskNinjaHub.Application.Entities.InformationSystem.Domain.InformationSystem}" />
/// </summary>
/// <seealso cref="TaskNinjaHub.Application.Entities.Bases.Interfaces.IBaseRepository{TaskNinjaHub.Application.Entities.InformationSystem.Domain.InformationSystem}" />
public interface IInformationSystemRepository: IBaseRepository<Domain.InformationSystem>
{

}