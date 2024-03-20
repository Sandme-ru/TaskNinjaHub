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

    public new async Task<OperationResult<Author>> AddAsync(Author author)
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

                return OperationResult<Author>.SuccessResult(updatedAuthor);
            }

            _context.Authors.Update(author);
            await ((DbContext)_context).SaveChangesAsync();

            return OperationResult<Author>.SuccessResult(author);
        }
        catch (Exception ex)
        {
            return OperationResult<Author>.FailedResult($"Failed to add author: {ex.Message}");
        }
    }
}