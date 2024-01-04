namespace TaskNinjaHub.Application.Utilities;

public static class UniqueNameGeneratorExtensions
{
    public static string GetUniqueFileName(this string fileName)
    {
        fileName = Path.GetFileName(fileName);
        
        return string.Concat(Path.GetFileNameWithoutExtension(fileName),
            "_",
            Guid.NewGuid().ToString().AsSpan(0, 4),
            Path.GetExtension(fileName));
    }
}