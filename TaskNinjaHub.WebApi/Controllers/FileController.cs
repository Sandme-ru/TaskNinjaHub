using Microsoft.AspNetCore.Mvc;
using TaskNinjaHub.Application.Entities.Files.Dto;
using TaskNinjaHub.Application.Entities.Files.Interfaces;
using TaskNinjaHub.Application.Utilities;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;

namespace TaskNinjaHub.WebApi.Controllers;

/// <summary>
/// Class FileController.
/// Implements the <see cref="IFileRepository" />
/// </summary>
/// <seealso cref="IFileRepository" />
public class FileController : ControllerBase
{
    /// <summary>
    /// The repository
    /// </summary>
    private readonly IFileRepository _repository;

    /// <summary>
    /// The environment
    /// </summary>
    private readonly IWebHostEnvironment _environment;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileController" /> class.
    /// </summary>
    /// <param name="repository">The repository.</param>
    /// <param name="environment">The WebHostEnvironment.</param>
    public FileController(IFileRepository repository, IWebHostEnvironment environment)
    {
        _repository = repository;
        _environment = environment;
    }

    /// <summary>
    /// Gets all files by task id.
    /// </summary>
    /// <param name="taskId">The task identifier.</param>
    /// <returns>System.Nullable&lt;IEnumerable&lt;File&gt;&gt;.</returns>
    [HttpGet("/api/file")]
    public async Task<IEnumerable<File?>> GetAllByTaskId(
        [FromQuery] int taskId)
    {
        if (taskId <= 0)
            return null!;

        var files = await _repository.Find(f => f.TaskId == taskId);

        return files ?? Array.Empty<File>();
    }

    /// <summary>
    /// Download file by path.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>System.Nullable&lt;File&gt;&gt;.</returns>
    [HttpGet("{*path}")]
    public async Task<ActionResult<File?>?> Download(
        [FromRoute] string path)
    {
        if (path is null or { Length: 0 })
            return null;

        var files = await _repository.GetAll();
        var file = files!.FirstOrDefault(f => f.Path == path);
        if (file is null)
            return null;

        var filePath = Path.Combine(_environment.WebRootPath, file.Path!);
        var fileData = await System.IO.File.ReadAllBytesAsync(filePath);

        return File(fileData, "application/octet-stream");
    }

    /// <summary>
    /// Upload file with form data.
    /// </summary>
    /// <returns>System.Nullable&lt;File&gt;&gt;.</returns>
    [HttpPost("/api/file/upload")]
    [RequestSizeLimit(8 * 1024 * 1024)]
    public async Task<File?> Upload(
        [FromForm] IFormFile? file,
        [FromQuery] int? taskId,
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

        if (string.IsNullOrEmpty(_environment.WebRootPath))
        {
            _environment.WebRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
            Directory.CreateDirectory(Path.GetDirectoryName(_environment.WebRootPath) ??
                                      throw new("Directory name is null"));
        }

        var directoryName = Path.Combine(_environment.WebRootPath, "files");
        var filePath = Path.Combine(directoryName, fileName);

        var fileEntity = new File
        {
            Name = fileName,
            Path = $"files/{fileName}",
            DateCreated = DateTime.UtcNow,
            TaskId = taskId
        };

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? throw new("Directory name is null"));

            await _repository.Add(fileEntity);

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

    /// <summary>
    /// Change files owner.
    /// </summary>
    /// <returns>System.Nullable&lt;File&gt;&gt;.</returns>
    [HttpPut("/api/file/owner-change")]
    public async Task<File?> OwnerChange(
        [FromBody] FileOwnershipDto fileOwnershipDto)
    {
        if (fileOwnershipDto.FileId == 0 || fileOwnershipDto.TaskId == 0)
            return null;

        var files = await _repository.GetAll();
        var file = files!.FirstOrDefault(f => f.Id == fileOwnershipDto.FileId);
        if (file is null)
            return null;

        file.TaskId = fileOwnershipDto.TaskId;

        await _repository.Update(file);

        return file;
    }

    /// <summary>
    /// Remove file by id.
    /// </summary>
    /// <returns>System.Nullable&lt;File&gt;&gt;.</returns>
    [HttpDelete("/api/file/{id}")]
    public async Task<ActionResult<File?>?> Remove([FromRoute] int id)
    {
        if (id <= 0)
            return null;

        var files = await _repository.GetAll();
        var file = files!.FirstOrDefault(f => f.Id == id);

        if (file is null)
            return null;

        var filePath = Path.Combine(_environment.WebRootPath, file.Path!);

        if (!System.IO.File.Exists(filePath))
            return null;

        System.IO.File.Delete(filePath);
        await _repository.Remove(file);

        return file;
    }
}