namespace TaskNinjaHub.Application.Utilities;

/// <summary>
/// Class UniqueNameGeneratorExtensions.
/// </summary>
public static class UniqueNameGeneratorExtensions
{
    /// <summary>
    /// Gets the name of the unique file.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <returns>System.String.</returns>
    public static string GetUniqueFileName(this string fileName)
    {
        fileName = Path.GetFileName(fileName);
        
        return string.Concat(Path.GetFileNameWithoutExtension(fileName),
            "_",
            Guid.NewGuid().ToString().AsSpan(0, 4),
            Path.GetExtension(fileName));
    }
}