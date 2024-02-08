using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.Authors.Interfaces;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Interfaces;
using TaskNinjaHub.Application.Utilities.OperationResults;

namespace TaskNinjaHub.Application.Entities.Authors.Repositories;

public class AuthorRepository(ITaskNinjaHubDbContext? context) : BaseRepository<Author>((DbContext)context!), IAuthorRepository
{
    private readonly ITaskNinjaHubDbContext _context = context!;

    public new async Task<OperationResult> AddAsync(Author author)
    {
        try
        {
            var updatedAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Name == author.Name);

            if (updatedAuthor != null)
            {
                updatedAuthor.Name = author.Name;
                updatedAuthor.RoleName = author.RoleName;
                updatedAuthor.ShortName = author.ShortName;

                _context.Authors.Update(updatedAuthor);
                await ((DbContext)_context).SaveChangesAsync();

                return OperationResult.SuccessResult();
            }

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