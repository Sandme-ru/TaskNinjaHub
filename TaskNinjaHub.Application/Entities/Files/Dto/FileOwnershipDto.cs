namespace TaskNinjaHub.Application.Entities.Files.Dto;

/// <summary>
/// Class FileOwnershipDto.
/// </summary>
public class FileOwnershipDto
{
    /// <summary>
    /// Gets or sets the file identifier.
    /// </summary>
    /// <value>The file identifier.</value>
    public int FileId { get; set; }
    /// <summary>
    /// Gets or sets the task identifier.
    /// </summary>
    /// <value>The task identifier.</value>
    public int TaskId { get; set; }
}