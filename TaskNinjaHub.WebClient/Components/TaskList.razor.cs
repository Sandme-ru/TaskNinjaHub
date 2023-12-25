using System.Text.Json;
using AntDesign;
using DiffMatchPatch;
using Microsoft.AspNetCore.Components;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.Users.Domain;
using TaskNinjaHub.WebClient.Services;
using TaskNinjaHub.WebClient.Services.Bases;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;
using TaskStatus = TaskNinjaHub.Application.Entities.TaskStatuses.Domain.TaskStatus;

namespace TaskNinjaHub.WebClient.Components;

/// <summary>
/// Class TaskList.
/// Implements the <see cref="ComponentBase" />
/// </summary>
/// <seealso cref="ComponentBase" />
public partial class TaskList
{
    #region INJECTIONS

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    /// <value>The message.</value>
    [Inject]
    private IMessageService Message { get; set; } = null!;

    /// <summary>
    /// Gets or sets the modal service.
    /// </summary>
    /// <value>The modal service.</value>
    [Inject]
    private ModalService ModalService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the file service.
    /// </summary>
    /// <value>The file service.</value>
    [Inject]
    private FileService FileService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the task service.
    /// </summary>
    /// <value>The task service.</value>
    [Inject]
    private CatalogTaskService CatalogTaskService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the task status service.
    /// </summary>
    /// <value>The task status service.</value>
    [Inject]
    private TaskStatusService TaskStatusService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the role service.
    /// </summary>
    /// <value>The role service.</value>
    [Inject]
    private RoleService RoleService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the priority service.
    /// </summary>
    /// <value>The priority service.</value>
    [Inject]
    private PriorityService PriorityService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the information system service.
    /// </summary>
    /// <value>The information system service.</value>
    [Inject]
    private InformationSystemService InformationSystemService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the author service.
    /// </summary>
    /// <value>The author service.</value>
    [Inject]
    private AuthorService AuthorService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the navigation manager.
    /// </summary>
    /// <value>The navigation manager.</value>
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the message service.
    /// </summary>
    /// <value>The message service.</value>
    [Inject]
    private IMessageService MessageService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user authentication service.
    /// </summary>
    /// <value>The user authentication service.</value>
    [Inject]
    private IUserAuthenticationService UserAuthenticationService { get; set; } = null!;

    #endregion

    #region PROPERTY

    /// <summary>
    /// Gets the bool definition visibility preview.
    /// </summary>
    /// <value>The preview visibility bool.</value>
    private bool IsPreviewVisible { get; set; }

    /// <summary>
    /// Gets the file preview url string.
    /// </summary>
    /// <value>The file preview url.</value>
    private string? FilePreviewUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets the file preview title string.
    /// </summary>
    /// <value>The file preview title.</value>
    private string? FilePreviewTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets the current user.
    /// </summary>
    /// <value>The current user.</value>
    private User? CurrentUser => UserAuthenticationService.AuthorizedUser;

    /// <summary>
    /// Gets or sets the selected catalog task.
    /// </summary>
    /// <value>The selected catalog task.</value>
    private CatalogTask? SelectedCatalogTask { get; set; }

    /// <summary>
    /// The catalog tasks for changelog
    /// </summary>
    /// <value>The catalog tasks for changelog</value>
    private List<CatalogTask>? CatalogTasksForChangelog { get; set; }

    /// <summary>
    /// Gets or sets the edited task.
    /// </summary>
    /// <value>The edited task.</value>
    private CatalogTask? EditedTask { get; set; }

    /// <summary>
    /// The clone of edited task.
    /// </summary>
    private CatalogTask? _cloneTask { get; set; }

    /// <summary>
    /// Gets or sets the tasks for changelog.
    /// </summary>
    /// <value>The tasks for changelog</value>
    private CatalogTask? CatalogTaskForChangelog { get; set; }

    /// <summary>
    /// Gets or sets the html markup strings.
    /// </summary>
    /// <value>Html markup strings.</value>
    private List<string> HtmlMarkupForTask { get; set; }

    /// <summary>
    /// The catalog tasks
    /// </summary>
    /// <value>The catalog tasks.</value>
    private List<CatalogTask>? CatalogTasks { get; set; } = new();

    /// <summary>
    /// Gets or sets the default files list.
    /// </summary>
    /// <value>The default files list.</value>
    private List<UploadFileItem>? DefaultFileList { get; set; } = new();

    /// <summary>
    /// The grid
    /// </summary>
    private readonly ListGridType _grid = new()
    {
        Gutter = 16,
        Xs = 1,
        Sm = 2,
        Md = 4,
        Lg = 4,
        Xl = 6,
        Xxl = 3,
    };

    /// <summary>
    /// The placement
    /// </summary>
    private string _placement = "right";

    /// <summary>
    /// The visible
    /// </summary>
    private bool _visibleDrawer = false;

    /// <summary>
    /// The visible modal
    /// </summary>
    private bool _visibleModal = false;

    /// <summary>
    /// Gets or sets the authors.
    /// </summary>
    /// <value>The authors.</value>
    private List<CatalogTask> CatalogTasksList { get; set; } = new();

    /// <summary>
    /// Gets or sets the authors.
    /// </summary>
    /// <value>The authors.</value>
    private List<Author> AuthorsList { get; set; } = new();

    /// <summary>
    /// Gets or sets the priority list.
    /// </summary>
    /// <value>The priority list.</value>
    private List<Priority> PriorityList { get; set; } = new();

    /// <summary>
    /// Gets or sets the information system list.
    /// </summary>
    /// <value>The information system list.</value>
    private List<InformationSystem> InformationSystemList { get; set; } = new();

    /// <summary>
    /// Gets or sets the task status list.
    /// </summary>
    /// <value>The task status list.</value>
    private List<TaskStatus> TaskStatusList { get; set; } = new();

    /// <summary>
    /// Gets or sets the deleted task.
    /// </summary>
    /// <value>The deleted task.</value>
    private static CatalogTask? DeletedTask { get; set; }

    /// <summary>
    /// The on ok
    /// </summary>
    private Func<ModalClosingEventArgs, Task> _onOk;

    /// <summary>
    /// Gets or sets the task information system by which tasks will be selected.
    /// </summary>
    /// <value>The task information system.</value>
    private InformationSystem? SelectedInformationSystem { get; set; }

    /// <summary>
    /// Gets or sets the task author by which tasks will be selected.
    /// </summary>
    /// <value>The task author.</value>
    private Author? SelectedAuthor { get; set; }

    /// <summary>
    /// Gets or sets the task executor by which tasks will be selected.
    /// </summary>
    /// <value>The task executor.</value>
    private Author? SelectedExecutor { get; set; }

    /// <summary>
    /// Gets or sets the task identifier by which tasks will be selected.
    /// </summary>
    /// <value>The task id.</value>
    private int? SelectedTaskId { get; set; }

    #endregion

    /// <summary>
    /// On initialized as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        //FillingTestCards();

        AuthorsList = (await AuthorService.GetAllAsync() ?? Array.Empty<Author>()).ToList();
        PriorityList = (await PriorityService.GetAllAsync() ?? Array.Empty<Priority>()).ToList();
        InformationSystemList =
            (await InformationSystemService.GetAllAsync() ?? Array.Empty<InformationSystem>()).ToList();
        TaskStatusList = (await TaskStatusService.GetAllAsync() ?? Array.Empty<TaskStatus>()).ToList();
        CatalogTasks = (await CatalogTaskService.GetAllAsync() ?? Array.Empty<CatalogTask>()).ToList();

        StateHasChanged();
    }

    /// <summary>
    /// Deletes the task handler.
    /// </summary>
    private async Task DeleteTaskHandler()
    {
        if (DeletedTask != null)
        {
            if (CatalogTasks != null)
            {
                foreach (var task in CatalogTasks.Where(t => t.OriginalTaskId == DeletedTask.Id))
                    await CatalogTaskService.DeleteAsync(task.Id);

                var response = await CatalogTaskService.DeleteAsync(DeletedTask.Id);
                if (response.IsSuccessStatusCode)
                {
                    CatalogTasks.Remove(DeletedTask);
                    await MessageService.Success("Deletion completed successfully.");
                }
                else
                    await MessageService.Error("Deletion failed.");
            }
        }

        StateHasChanged();
    }

    /// <summary>
    /// The on cancel
    /// </summary>
    private Func<ModalClosingEventArgs, Task> _onCancel = (e) =>
    {
        Console.WriteLine("Cancel");
        return Task.CompletedTask;
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskList" /> class.
    /// </summary>
    public TaskList()
    {
        _onOk = async (e) =>
        {
            await DeleteTaskHandler();
            Console.WriteLine("OK");
        };
    }

    /// <summary>
    /// Fillings the test cards.
    /// </summary>
    private void FillingTestCards()
    {
        CatalogTasks?.Add(new CatalogTask()
        {
            Id = 0,
            Name = "Lorem ipsum dolor sit amet.",
            Description =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            InformationSystem = new InformationSystem()
            {
                Name = "Sed ut perspiciatis"
            },
            Priority = new Priority
            {
                Name = "Sed ut perspiciatis"
            },
            TaskAuthor = new Author
            {
                Name = "Lorem ipsum dolor sit amet"
            },
            TaskExecutor = new Author
            {
                Name = "Lorem ipsum dolor sit amet"
            }
        });
        CatalogTasks?.Add(new CatalogTask
        {
            Id = 1,
            Name = "Lorem ipsum dolor sit amet.",
            Description =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            InformationSystem = new InformationSystem
            {
                Name = "Sed ut perspiciatis"
            },
            Priority = new Priority
            {
                Name = "Sed ut perspiciatis"
            },
            TaskAuthor = new Author
            {
                Name = "Lorem ipsum dolor sit amet"
            },
            TaskExecutor = new Author
            {
                Name = "Lorem ipsum dolor sit amet"
            }
        });
        CatalogTasks?.Add(new CatalogTask
        {
            Id = 2,
            Name = "Lorem ipsum dolor sit amet.",
            Description =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            InformationSystem = new InformationSystem()
            {
                Name = "Sed ut perspiciatis"
            },
            Priority = new Priority
            {
                Name = "Sed ut perspiciatis"
            },
            TaskAuthor = new Author
            {
                Name = "Lorem ipsum dolor sit amet"
            },
            TaskExecutor = new Author
            {
                Name = "Lorem ipsum dolor sit amet"
            }
        });
        CatalogTasks?.Add(new CatalogTask
        {
            Id = 3,
            Name = "Lorem ipsum dolor sit amet.",
            Description =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            InformationSystem = new InformationSystem
            {
                Name = "Sed ut perspiciatis"
            },
            Priority = new Priority
            {
                Name = "Sed ut perspiciatis"
            },
            TaskAuthor = new Author
            {
                Name = "Lorem ipsum dolor sit amet"
            },
            TaskExecutor = new Author
            {
                Name = "Lorem ipsum dolor sit amet"
            }
        });
        CatalogTasks?.Add(new CatalogTask()
        {
            Id = 4,
            Name = "Lorem ipsum dolor sit amet.",
            Description =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            InformationSystem = new InformationSystem
            {
                Name = "Sed ut perspiciatis"
            },
            Priority = new Priority
            {
                Name = "Sed ut perspiciatis"
            },
            TaskAuthor = new Author
            {
                Name = "Lorem ipsum dolor sit amet"
            },
            TaskExecutor = new Author
            {
                Name = "Lorem ipsum dolor sit amet"
            }
        });
    }

    /// <summary>
    /// Closes this instance.
    /// </summary>
    private void Close()
    {
        this._visibleDrawer = false;
    }

    /// <summary>
    /// Opens this instance.
    /// </summary>
    /// <param name="catalogTask">The catalog task.</param>
    /// <returns>Task.</returns>
    private async Task Open(CatalogTask catalogTask)
    {
        HtmlMarkupForTask = new List<string>();
        SelectedCatalogTask = catalogTask;
        var taskFiles = await FileService.GetAllByTaskIdAsync(catalogTask!.Id);
        if (taskFiles?.ToList() is not null and {Count: > 0} files)
        {
            catalogTask.Files = files;
            DefaultFileList = catalogTask.Files
                .Select(f => new UploadFileItem
                {
                    Id = $"{f.Id}",
                    FileName = f.Name,
                    Url = $"https://localhost:7033/{f.Path}", //TODO remove connection to localhost
                    ObjectURL = $"https://localhost:7033/{f.Path}",
                    Response = JsonSerializer.Serialize(
                        f,
                        new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase}),
                    State = UploadState.Success
                }).ToList()!;
        }
        else
            DefaultFileList = new(); 
        CatalogTasksForChangelog = CatalogTasks!.Where(t => t.OriginalTaskId == catalogTask.Id).OrderByDescending(t => t.DateCreated).ToList();
        if (CatalogTasksForChangelog.Any())
        {
            var prev = new CatalogTask();
            CatalogTasksForChangelog.Insert(0, catalogTask);

            foreach (var item in CatalogTasksForChangelog)
            {
                var header = "<h3>Changed:</h3><b>";
                var body = "";

                if (item == CatalogTasksForChangelog.First())
                    prev = item;
                else
                {

                    body += $"<p>Edit date: {item.DateCreated} Editor: {item.UserCreated}</p>";

                    if (prev.Name != item.Name)
                    {
                        header += "<p>Name<p>";
                        body += $"<p>Name: {item.Name}&rarr;{prev.Name}</p>";
                    }
                    else
                    {
                        body += $"<p>Name: {item.Name}</p>";
                    }
                    if (prev.Description != item.Description)
                    {
                        header += "<p>Description<p>";
                        body += TextCompare(item.Description, prev.Description);
                    }
                    else
                    {
                        body += TextCompare(item.Description, prev.Description);
                    }
                    if (prev.InformationSystemId != item.InformationSystemId)
                    {
                        header += "<p>Information system<p>";
                        body += $"<p>Information system: {item.InformationSystem?.Name}&rarr;{prev.InformationSystem?.Name}</p>";
                    }
                    else
                    {
                        body += $"<p>Information system: {item.InformationSystem?.Name}</p>";
                    }
                    if (prev.TaskExecutorId != item.TaskExecutorId)
                    {
                        header += "<p>Task executor<p>";
                        body += $"<p>Task executor: {item.TaskExecutor?.Name}&rarr;{prev.TaskExecutor?.Name}</p>";
                    }
                    else
                    {
                        body += $"<p>Task executor: {item.TaskExecutor?.Name}</p>";
                    }
                    if (prev.PriorityId != item.PriorityId)
                    {
                        header += "<p>Priority</p>";
                        body += $"<p>Priority: {item.Priority?.Name}&rarr;{prev.Priority?.Name}</p>";
                    }
                    else
                    {
                        body += $"<p>Priority: {item.Priority?.Name}</p>";
                    }
                    if (prev.TaskStatusId != item.TaskStatusId)
                    {
                        header += "<p>Task Status</p>";
                        body += $"<p>Task status: {item.TaskStatus?.Name}&rarr;{prev.TaskStatus?.Name}</p>";
                    }
                    else
                    {
                        body += $"<p>Task status: {item.TaskStatus?.Name}</p>";
                    }
                    var result = header + "</b>" + body;
                    HtmlMarkupForTask.Add(result);

                    prev = item;
                }

            }
        }
        _visibleDrawer = true;
        StateHasChanged();
    }

    /// <summary>
    /// Edits the task.
    /// </summary>
    /// <param name="catalogTask">The catalog task.</param>
    /// <returns>Task.</returns>
    private async Task EditTaskEnabled(CatalogTask? catalogTask)
    {
        EditedTask = catalogTask;
        var taskFiles = await FileService.GetAllByTaskIdAsync(catalogTask!.Id);
        if (taskFiles?.ToList() is not null and {Count: > 0} files)
        {
            catalogTask.Files = files;
            DefaultFileList = catalogTask.Files
                .Select(f => new UploadFileItem
                {
                    Id = $"{f.Id}",
                    FileName = f.Name,
                    Url = $"https://localhost:7033/{f.Path}", //TODO remove connection to localhost
                    ObjectURL = $"https://localhost:7033/{f.Path}",
                    Response = JsonSerializer.Serialize(
                        f,
                        new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase}),
                    State = UploadState.Success
                }).ToList()!;
        }
        else
        {
            EditedTask!.Files = new List<File>();
            DefaultFileList = new List<UploadFileItem>();
        }
        
        _cloneTask = new CatalogTask
        {
            Name = catalogTask.Name,
            Description = catalogTask.Description,
            TaskAuthorId = catalogTask.TaskAuthorId,
            TaskExecutorId = catalogTask.TaskExecutorId,
            InformationSystemId = catalogTask.InformationSystemId,
            PriorityId = catalogTask.PriorityId,
            TaskStatusId = catalogTask.TaskStatusId,
        };
        _visibleModal = true;
        StateHasChanged();
    }

    /// <summary>
    /// Handles the cancel.
    /// </summary>
    /// <returns>Task.</returns>
    private Task HandleCancel()
    {
        if (EditedTask != null)
        {
            EditedTask.Name = _cloneTask?.Name ?? string.Empty;
            EditedTask.Description = _cloneTask?.Description;
            EditedTask.TaskAuthorId = _cloneTask?.TaskAuthorId;
            EditedTask.TaskExecutorId = _cloneTask?.TaskExecutorId;
            EditedTask.InformationSystemId = _cloneTask?.InformationSystemId;
            EditedTask.PriorityId = _cloneTask?.PriorityId;
            EditedTask.TaskStatusId = _cloneTask?.TaskStatusId;
        }

        _visibleModal = false;
        StateHasChanged();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Edits the task.
    /// </summary>
    private async Task EditTask()
    {
        if (EditedTask != null)
        {
            if (EditedTask.Name == _cloneTask?.Name
                && EditedTask.Description == _cloneTask.Description
                && EditedTask.TaskAuthorId == _cloneTask.TaskAuthorId
                && EditedTask.TaskExecutorId == _cloneTask.TaskExecutorId
                && EditedTask.InformationSystemId == _cloneTask.InformationSystemId
                && EditedTask.PriorityId == _cloneTask.PriorityId
                && EditedTask.TaskStatusId == _cloneTask.TaskStatusId
                && Equals(EditedTask.Files?.ToArray(), DefaultFileList?.ToArray()))
            {
                await MessageService.Error("Change one or more attributes");
            }
            else
            {
                Console.WriteLine($"{EditedTask?.Id} {EditedTask?.Name} is updated.");
                UpdatingDependentAttributes();
                EditedTask.UserUpdated = CurrentUser?.Username;
                EditedTask.DateUpdated = DateTime.Now;

                var changeRet = await CatalogTaskService.UpdateAsync(EditedTask!);
                if (changeRet.IsSuccessStatusCode)
                {
                    Console.WriteLine($"{EditedTask?.Id} {EditedTask?.Name} is updated.");
                    if (EditedTask.Files is {Count: > 0})
                    {
                        var createdTaskStream = await changeRet.Content.ReadAsStreamAsync();
                        var createdTask = await JsonSerializer.DeserializeAsync<CatalogTask>(
                            createdTaskStream,
                            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
                        foreach (var fileEntity in EditedTask.Files!)
                            await FileService.ChangeOwnershipAsync(fileEntity.Id, createdTask!.Id);
                    }
                    else
                        foreach (var fileEntity in EditedTask.Files!)
                            await FileService.DeleteAsync(fileEntity.Id);
                }

                CatalogTaskForChangelog = new CatalogTask
                {
                    Name = _cloneTask.Name,
                    Description = _cloneTask.Description,
                    TaskAuthorId = _cloneTask.TaskAuthorId,
                    TaskExecutorId = _cloneTask.TaskExecutorId,
                    InformationSystemId = _cloneTask.InformationSystemId,
                    PriorityId = _cloneTask.PriorityId,
                    TaskStatusId = _cloneTask.TaskStatusId,
                    OriginalTaskId = EditedTask.Id,
                    DateCreated = EditedTask.DateUpdated,
                    UserCreated = EditedTask.UserUpdated,
                    Files = EditedTask.Files
                };

                var createRet = await CatalogTaskService.CreateAsync(CatalogTaskForChangelog);
                if (createRet.IsSuccessStatusCode)
                {
                    Console.WriteLine($"{EditedTask?.Id} {EditedTask?.Name} is updated.");
                    if (CatalogTaskForChangelog.Files is {Count: > 0})
                    {
                        var createdTaskStream = await changeRet.Content.ReadAsStreamAsync();
                        var createdTask = await JsonSerializer.DeserializeAsync<CatalogTask>(
                            createdTaskStream,
                            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
                        foreach (var fileEntity in CatalogTaskForChangelog.Files!)
                            await FileService.ChangeOwnershipAsync(fileEntity.Id, createdTask!.Id);
                    }
                    else
                        foreach (var fileEntity in CatalogTaskForChangelog.Files!)
                            await FileService.DeleteAsync(fileEntity.Id);

                    CatalogTasks = (await CatalogTaskService.GetAllAsync() ?? Array.Empty<CatalogTask>()).ToList();
                }
                _visibleModal = false;
                StateHasChanged();
            }
        }
    }

    /// <summary>
    /// Updatings the dependent attributes.
    /// </summary>
    private void UpdatingDependentAttributes()
    {
        if (EditedTask == null)
            return;

        EditedTask.TaskAuthor = AuthorsList.Find(a => a.Id == EditedTask.TaskAuthorId);
        EditedTask.TaskExecutor = AuthorsList.Find(a => a.Id == EditedTask.TaskExecutorId);
        EditedTask.InformationSystem = InformationSystemList.Find(a => a.Id == EditedTask.InformationSystemId);
        EditedTask.Priority = PriorityList.Find(a => a.Id == EditedTask.PriorityId);
        EditedTask.TaskStatus = TaskStatusList.Find(a => a.Id == EditedTask.TaskStatusId);
    }

    /// <summary>
    /// Deletes the task.
    /// </summary>
    /// <param name="task">The task.</param>
    private void DeleteTask(CatalogTask task)
    {
        DeletedTask = task;
        ModalService?.Confirm(new ConfirmOptions()
        {
            Title = $"Are you sure delete this task {task.Id} {task.Name}?",
            Icon = _icon,
            Content = "Some descriptions",
            OnOk = _onOk,
            OnCancel = _onCancel,
            OkType = "danger"
        });
    }

    /// <summary>
    /// Updating the list of tasks for changing filter properties.
    /// </summary>
    private async Task FilterSelectionChanged()
    {
        var filter = new CatalogTask()
        {
            Id = SelectedTaskId ?? 0,
            InformationSystemId = SelectedInformationSystem?.Id,
            TaskAuthorId = SelectedAuthor?.Id,
            TaskExecutorId = SelectedExecutor?.Id
        };

        var result = (await CatalogTaskService.GetAllByFilterAsync(filter))!.Where(t => t.OriginalTaskId == null).ToList();
        CatalogTasks = new List<CatalogTask>(result);
    }

    /// <summary>
    /// Called before file upload to server.
    /// </summary>
    /// <param name="file">The input file.</param>
    /// <returns>Task.</returns>
    private bool BeforeUpload(UploadFileItem file)
    {
        var isLt8M = file.Size / 1024 / 1024 < 8;

        if (!isLt8M)
            Message.Error("File must smaller than 8MB!");

        return isLt8M;
    }

    /// <summary>
    /// Called after file change.
    /// </summary>
    /// <param name="fileInfo">The input file info.</param>
    /// <returns>Task.</returns>
    private void HandleChange(UploadInfo fileInfo)
    {
        if (fileInfo.File.State != UploadState.Success) return;
        var uploadedFile = fileInfo.File.GetResponse<File>(new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        if (uploadedFile is not null)
            EditedTask?.Files?.Add(uploadedFile);
    }

    /// <summary>
    /// Called on file preview.
    /// </summary>
    /// <param name="file">The input file .</param>
    /// <returns>Task.</returns>
    private void OnPreview(UploadFileItem file)
    {
        if (!file.IsPicture())
        {
            NavigationManager.NavigateTo(file.ObjectURL,true);
            return;
        }
        IsPreviewVisible = true;
        FilePreviewTitle = file.FileName;
        FilePreviewUrl = file.ObjectURL;
    }

    /// <summary>
    /// Called on file remove.
    /// </summary>
    /// <param name="file">The input file .</param>
    /// <returns>Task.</returns>
    private async Task<bool> OnRemove(UploadFileItem file)
    {
        var uploadedFile = file.GetResponse<File>(new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var fileToRemove = EditedTask?.Files?.FirstOrDefault(f => f.Id == uploadedFile!.Id);
        if (fileToRemove is null)
            return false;
        DefaultFileList?.Remove(file);
        EditedTask?.Files?.Remove(fileToRemove);
        var res = await FileService.DeleteAsync(fileToRemove.Id);
        return res.IsSuccessStatusCode;
    }
    
    /// Finding difference between two strings.
    /// </summary>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    /// <returns>Html markup.</returns>
    private string TextCompare(string oldValue, string newValue)
    {
        diff_match_patch dmp = new diff_match_patch();
        List<Diff> diff = dmp.diff_main(oldValue, newValue);
        dmp.diff_cleanupSemantic(diff);
        string result = dmp.diff_prettyHtml(diff);
        return $"<p>Description: {result}</p>";
    }
}