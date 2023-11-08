using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.Bases.Interfaces;

namespace TaskNinjaHub.Application.Entities.Authors.Interfaces;

/// <summary>
/// Interface IAuthorRepository
/// Extends the <see cref="Author" />
/// </summary>
/// <seealso cref="Author" />
public interface IAuthorRepository : IBaseRepository<Author>
{

}