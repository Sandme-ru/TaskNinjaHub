using System.Text.Json;
using AntDesign;
using DiffMatchPatch;
using Microsoft.AspNetCore.Components;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.WebClient.Services;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;
using TaskStatus = TaskNinjaHub.Application.Entities.TaskStatuses.Domain.TaskStatus;

namespace TaskNinjaHub.WebClient.Components;

public partial class TaskList
{
    #region INJECTIONS

    [Inject]
    private IMessageService Message { get; set; } = null!;

    [Inject]
    private ModalService ModalService { get; set; } = null!;

    [Inject]
    private FileService FileService { get; set; } = null!;

    [Inject]
    private CatalogTaskService CatalogTaskService { get; set; } = null!;

    [Inject]
    private TaskStatusService TaskStatusService { get; set; } = null!;

    [Inject]
    private PriorityService PriorityService { get; set; } = null!;

    [Inject]
    private InformationSystemService InformationSystemService { get; set; } = null!;

    [Inject]
    private AuthorService AuthorService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private IMessageService MessageService { get; set; } = null!;

    [Inject]
    private IUserProviderService UserProviderService { get; set; } = null!;

    #endregion

    #region PROPERTY

    private bool IsPreviewVisible { get; set; }

    private string? FilePreviewUrl { get; set; } = string.Empty;

    private string? FilePreviewTitle { get; set; } = string.Empty;

    private string? CurrentUser
    {
        get => UserProviderService.User.Name;
        set => UserProviderService.User.Name = value;
    }

    private CatalogTask? SelectedCatalogTask { get; set; }

    private List<CatalogTask>? CatalogTasksForChangelog { get; set; }

    private CatalogTask? EditedTask { get; set; }

    private CatalogTask? CloneTask { get; set; }

    private CatalogTask? CatalogTaskForChangelog { get; set; }

    private List<string> HtmlMarkupForTask { get; set; } = null!;

    private List<CatalogTask>? CatalogTasks { get; set; } = new();

    private List<UploadFileItem>? DefaultFileList { get; set; } = new();

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

    private string _placement = "right";

    private bool _visibleDrawer = false;

    private bool _visibleModal = false;

    private List<CatalogTask> CatalogTasksList { get; set; } = new();

    private List<Author> AuthorsList { get; set; } = new();

    private List<Priority> PriorityList { get; set; } = new();

    private List<InformationSystem> InformationSystemList { get; set; } = new();

    private List<TaskStatus> TaskStatusList { get; set; } = new();

    private static CatalogTask? DeletedTask { get; set; }

    private Func<ModalClosingEventArgs, Task> _onOk;

    private InformationSystem? SelectedInformationSystem { get; set; }

    private Author? SelectedAuthor { get; set; }

    private Author? SelectedExecutor { get; set; }

    private int? SelectedTaskId { get; set; }

    #endregion

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

    private Func<ModalClosingEventArgs, Task> _onCancel = (e) =>
    {
        Console.WriteLine("Cancel");
        return Task.CompletedTask;
    };

    public TaskList()
    {
        _onOk = async (e) =>
        {
            await DeleteTaskHandler();
            Console.WriteLine("OK");
        };
    }

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

    private void Close()
    {
        this._visibleDrawer = false;
    }

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
                    Url = $"https://localhost:7179/{f.Path}", //TODO remove connection to localhost
                    ObjectURL = $"https://localhost:7179/{f.Path}",
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
                    Url = $"https://localhost:7179/{f.Path}", //TODO remove connection to localhost
                    ObjectURL = $"https://localhost:7179/{f.Path}",
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
        
        CloneTask = new CatalogTask
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

    private Task HandleCancel()
    {
        if (EditedTask != null)
        {
            EditedTask.Name = CloneTask?.Name ?? string.Empty;
            EditedTask.Description = CloneTask?.Description;
            EditedTask.TaskAuthorId = CloneTask?.TaskAuthorId;
            EditedTask.TaskExecutorId = CloneTask?.TaskExecutorId;
            EditedTask.InformationSystemId = CloneTask?.InformationSystemId;
            EditedTask.PriorityId = CloneTask?.PriorityId;
            EditedTask.TaskStatusId = CloneTask?.TaskStatusId;
        }

        _visibleModal = false;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private async Task EditTask()
    {
        if (EditedTask != null)
        {
            if (EditedTask.Name == CloneTask?.Name
                && EditedTask.Description == CloneTask?.Description
                && EditedTask.TaskAuthorId == CloneTask?.TaskAuthorId
                && EditedTask.TaskExecutorId == CloneTask?.TaskExecutorId
                && EditedTask.InformationSystemId == CloneTask?.InformationSystemId
                && EditedTask.PriorityId == CloneTask?.PriorityId
                && EditedTask.TaskStatusId == CloneTask?.TaskStatusId
                && Equals(EditedTask.Files?.ToArray(), DefaultFileList?.ToArray()))
            {
                await MessageService.Error("Change one or more attributes");
            }
            else
            {
                Console.WriteLine($"{EditedTask?.Id} {EditedTask?.Name} is updated.");

                UpdatingDependentAttributes();

                EditedTask!.UserUpdated = CurrentUser;
                EditedTask.DateUpdated = DateTime.Now;

                var changeRet = await CatalogTaskService.UpdateAsync(EditedTask!);
                if (changeRet.IsSuccessStatusCode)
                {
                    Console.WriteLine($"{EditedTask?.Id} {EditedTask?.Name} is updated.");

                    if (EditedTask?.Files is {Count: > 0})
                    {
                        var createdTaskStream = await changeRet.Content.ReadAsStreamAsync();
                        var createdTask = await JsonSerializer.DeserializeAsync<CatalogTask>(
                            createdTaskStream,
                            new JsonSerializerOptions
                            {
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                            });
                        foreach (var fileEntity in EditedTask.Files!)
                            await FileService.ChangeOwnershipAsync(fileEntity.Id, createdTask!.Id);
                    }
                    else
                        foreach (var fileEntity in EditedTask?.Files!)
                            await FileService.DeleteAsync(fileEntity.Id);
                }

                CatalogTaskForChangelog = new CatalogTask
                {
                    Name = CloneTask?.Name,
                    Description = CloneTask?.Description,
                    TaskAuthorId = CloneTask?.TaskAuthorId,
                    TaskExecutorId = CloneTask?.TaskExecutorId,
                    InformationSystemId = CloneTask?.InformationSystemId,
                    PriorityId = CloneTask?.PriorityId,
                    TaskStatusId = CloneTask?.TaskStatusId,
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

    private void DeleteTask(CatalogTask task)
    {
        DeletedTask = task;
        ModalService?.Confirm(new ConfirmOptions
        {
            Title = $"Are you sure delete this task {task.Id} {task.Name}?",
            Icon = _icon,
            Content = "Some descriptions",
            OnOk = _onOk,
            OnCancel = _onCancel,
            OkType = "danger"
        });
    }

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

    private bool BeforeUpload(UploadFileItem file)
    {
        var isLt8M = file.Size / 1024 / 1024 < 8;

        if (!isLt8M)
            Message.Error("File must smaller than 8MB!");

        return isLt8M;
    }

    private void HandleChange(UploadInfo fileInfo)
    {
        if (fileInfo.File.State != UploadState.Success) 
            return;

        var uploadedFile = fileInfo.File.GetResponse<File>(new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

        if (uploadedFile is not null)
            EditedTask?.Files?.Add(uploadedFile);
    }

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
    
    private string TextCompare(string oldValue, string newValue)
    {
        var dmp = new diff_match_patch();
        List<Diff> diff = dmp.diff_main(oldValue, newValue);
        dmp.diff_cleanupSemantic(diff);
        var result = dmp.diff_prettyHtml(diff);
        return $"<p>Description: {result}</p>";
    }
}