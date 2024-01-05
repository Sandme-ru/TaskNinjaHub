using Microsoft.EntityFrameworkCore;
using System.Linq;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.Authors.Interfaces;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Interfaces;
using TaskNinjaHub.Application.Utilities.OperationResults;

namespace TaskNinjaHub.Application.Entities.Authors.Repositories;

public class AuthorRepository: BaseRepository<Author>, IAuthorRepository
{
    private readonly ITaskNinjaHubDbContext _context;

    public AuthorRepository(ITaskNinjaHubDbContext? context) : base((DbContext)context!)
    {
        _context = context!;
    }

    public new async Task<OperationResult> AddAsync(Author author)
    {
        try
        {
            if(_context.Authors.Any(a => a.Name == author.Name))
                return OperationResult.FailedResult($"Failed to add author");

            _context.Authors.Update(author);
            await ((DbContext)_context).SaveChangesAsync();

            return OperationResult.SuccessResult();
        }
        catch (Exception ex)
        {
            return OperationResult.FailedResult($"Failed to add author: {ex.Message}");
        }
    }
}