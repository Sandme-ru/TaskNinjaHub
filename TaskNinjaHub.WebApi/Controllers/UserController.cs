using Microsoft.AspNetCore.Mvc;
using TaskNinjaHub.Application.Entities.Users.Domain;
using TaskNinjaHub.Application.Entities.Users.Interfaces;
using TaskNinjaHub.WebApi.Controllers.Bases;

namespace TaskNinjaHub.WebApi.Controllers;

/// <summary>
/// Class UserController.
/// Implements the <see cref="IUserRepository" />
/// </summary>
/// <seealso cref="IUserRepository" />
public class UserController : BaseController<User, IUserRepository>
{
    /// <summary>
    /// The env
    /// </summary>
    private readonly IWebHostEnvironment _env;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UserController" /> class.
    /// </summary>
    /// <param name="repository">The repository.</param>
    /// <param name="env">The env.</param>
    public UserController(IUserRepository repository, IWebHostEnvironment env) : base(repository)
    {
        _env = env;
    }

    /// <summary>
    /// Saves the image.
    /// </summary>
    /// <param name="files">The files.</param>
    /// <returns>IFormFile.</returns>
    [HttpPost("UploadAvatar")]
    public async Task<IFormFile> UploadAvatar([FromForm] List<IFormFile> files)
    {
        if (!Directory.Exists(Path.Combine(_env.ContentRootPath, "resources")))
            Directory.CreateDirectory(Path.Combine(_env.ContentRootPath, "resources"));

        var path = Path.Combine(_env.ContentRootPath, "resources", files[0].FileName);

        await using FileStream fs = new(path, FileMode.Create);
        await files[0].CopyToAsync(fs);

        return files[0];
    }

    /// <summary>
    /// Retrieves image.
    /// </summary>
    /// <param name="avatarPath">Path to the image.</param>
    /// <returns>File.</returns>
    [HttpGet("GetAvatar/{avatarPath}")]
    public async Task<IActionResult> GetAvatar(string avatarPath)
    {
        if (!Directory.Exists(Path.Combine(_env.ContentRootPath, "resources")))
            Directory.CreateDirectory(Path.Combine(_env.ContentRootPath, "resources"));

        if (System.IO.File.Exists(Path.Combine(_env.ContentRootPath, "resources", avatarPath))) {
            var fileBytes = await System.IO.File.ReadAllBytesAsync(Path.Combine(_env.ContentRootPath, "resources", avatarPath));
            return File(fileBytes, "image/png");
        }

        else { return NotFound(); }
    }
}