using Microsoft.AspNetCore.Mvc;
using TaskNinjaHub.Application.Entities.Files.Dto;
using TaskNinjaHub.Application.Entities.Files.Interfaces;
using TaskNinjaHub.Application.Utilities;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;

namespace TaskNinjaHub.WebApi.Controllers;

public class FileController(IFileRepository repository, IWebHostEnvironment environment)
    : ControllerBase
{
    [HttpGet("/api/file")]
    public async Task<IEnumerable<File?>> GetAllByTaskId(
        [FromQuery] int taskId)
    {
        if (taskId <= 0)
            return null!;

        var files = await repository.FindAsync(f => f.TaskId == taskId);

        return files ?? Array.Empty<File>();
    }

    [HttpGet("{*path}")]
    public async Task<ActionResult<File?>?> Download(
        [FromRoute] string path)
    {
        if (path is null or { Length: 0 })
            return null;

        var files = await repository.GetAllAsync();
        var file = files!.FirstOrDefault(f => f.Path == path);
        if (file is null)
            return null;

        var filePath = Path.Combine(environment.WebRootPath, file.Path!);
        var fileData = await System.IO.File.ReadAllBytesAsync(filePath);

        return File(fileData, "application/octet-stream");
    }

    [HttpPost("/api/file/upload/{taskId}")]
    [RequestSizeLimit(8 * 1024 * 1024)]
    public async Task<File?> Upload(
        string? taskId,
        [FromForm] IFormFile? file,
        [FromQuery] string? fileName)
    {
        if (file is null or { Length: <= 0 })
            return null;

        fileName = fileName is null or { Length: 0 }
            ? file.FileName
            : Path.HasExtension(fileName)
                ? Path.ChangeExtension(fileName, Path.GetExtension(file.FileName))
                : $"{fileName}{Path.GetExtension(file.FileName)}";

        fileName = fileName.GetUniqueFileName();

        if (string.IsNullOrEmpty(environment.WebRootPath))
        {
            environment.WebRootPath = Path.Combine(environment.ContentRootPath, "wwwroot");
            Directory.CreateDirectory(Path.GetDirectoryName(environment.WebRootPath) ??
                                      throw new("Directory name is null"));
        }

        var directoryName = Path.Combine(environment.WebRootPath, "files");
        var filePath = Path.Combine(directoryName, fileName);

        var fileEntity = new File
        {
            Name = fileName,
            Path = $"files/{fileName}",
            DateCreated = DateTime.UtcNow,
            TaskId = Convert.ToInt32(taskId)
        };

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? throw new("Directory name is null"));

            await repository.AddAsync(fileEntity);

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            await using var stream = new FileStream(filePath, FileMode.CreateNew);
            await file.CopyToAsync(stream);

            return fileEntity;
        }
        catch
        {
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
            throw;
        }
    }

    [HttpPut("/api/file/owner-change")]
    public async Task<File?> OwnerChange(
        [FromBody] FileOwnershipDto fileOwnershipDto)
    {
        if (fileOwnershipDto.FileId == 0 || fileOwnershipDto.TaskId == 0)
            return null;

        var files = await repository.GetAllAsync();
        var file = files!.FirstOrDefault(f => f.Id == fileOwnershipDto.FileId);
        if (file is null)
            return null;

        file.TaskId = fileOwnershipDto.TaskId;

        await repository.UpdateAsync(file);

        return file;
    }

    [HttpDelete("/api/file/{id}")]
    public async Task<ActionResult<File?>?> Remove([FromRoute] int id)
    {
        if (id <= 0)
            return null;

        var files = await repository.GetAllAsync();
        var file = files!.FirstOrDefault(f => f.Id == id);

        if (file is null)
            return null;

        var filePath = Path.Combine(environment.WebRootPath, file.Path!);

        if (!System.IO.File.Exists(filePath))
            return null;

        System.IO.File.Delete(filePath);
        await repository.RemoveAsync(file);

        return file;
    }
}