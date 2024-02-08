using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.Authors.Interfaces;
using TaskNinjaHub.WebApi.Controllers.Bases;

namespace TaskNinjaHub.WebApi.Controllers;

public class AuthorController(IAuthorRepository repository) : BaseController<Author, IAuthorRepository>(repository);