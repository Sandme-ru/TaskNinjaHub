using TaskNinjaHub.Application.Entities.Files.Interfaces;
using TaskNinjaHub.WebApi.Controllers.Bases;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;

namespace TaskNinjaHub.WebApi.Controllers;

public class FileController(IFileRepository repository, IWebHostEnvironment environment) : BaseController<File, IFileRepository>(repository);