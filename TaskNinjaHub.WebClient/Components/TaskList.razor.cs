using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.RelatedTasks.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.TaskStatuses.Domain;
using TaskNinjaHub.Application.Filters;
using TaskNinjaHub.WebClient.Services;
using TaskNinjaHub.WebClient.Services.Bases;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;

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
    private RelatedTaskService RelatedTaskService { get; set; } = null!;

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

    private CatalogTask? EditedTask { get; set; }

    private CatalogTask? CloneTask { get; set; }

    private CatalogTask? CatalogTaskForChangelog { get; set; }

    private List<CatalogTask>? CatalogTasks { get; set; }

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

    private bool _visibleModal = false;

    private IEnumerable<Author>? AuthorsList { get; set; }

    private IEnumerable<Priority>? PriorityList { get; set; }

    private IEnumerable<InformationSystem>? InformationSystemList { get; set; }

    private IEnumerable<CatalogTaskStatus>? TaskStatusList { get; set; }

    private static CatalogTask? DeletedTask { get; set; }

    private Func<ModalClosingEventArgs, Task> _onOk;

    private InformationSystem? SelectedInformationSystem { get; set; }

    private Author? SelectedAuthor { get; set; }

    private Author? SelectedExecutor { get; set; }

    private File File { get; set; } = null!;

    private List<File> Files { get; set; } = null!;

    private int? SelectedTaskId { get; set; }

    private int PageSize { get; set; } = 10;

    private int CurrentPage { get; set; } = 1;

    private int CatalogTasksCount { get; set; }

    private bool IsLoadingTaskList { get; set; }

    private CatalogTask Filter { get; set; } = null!;

    public IEnumerable<int> RelatedTaskIds { get; set; }

    public IEnumerable<RelatedTask> RelatedTasks { get; set; }

    public IEnumerable<CatalogTask> CatalogTaskList { get; set; }

    private List<IBrowserFile> SelectedFiles = new List<IBrowserFile>();

    private HttpClient HttpClient { get; set; } = new();

    private bool PreviewVisible { get; set; } = false;

    private string PreviewTitle { get; set; } = string.Empty;
    private string ImgUrl { get; set; } = string.Empty;

    #endregion

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            IsLoadingTaskList = true;
            StateHasChanged();

            CatalogTasksCount = await CatalogTaskService.GetAllCountAsync();

            AuthorsList = await AuthorService.GetAllAsync();
            PriorityList = await PriorityService.GetAllAsync();
            InformationSystemList = await InformationSystemService.GetAllAsync();
            TaskStatusList = await TaskStatusService.GetAllAsync();
            CatalogTasks = (await CatalogTaskService.GetAllByPageAsync(new FilterModel { PageNumber = CurrentPage, PageSize = PageSize })).ToList();
            CatalogTaskList = (await CatalogTaskService.GetAllAsync()).Where(task => task.OriginalTaskId == null).ToList();

            IsLoadingTaskList = false;
            StateHasChanged();
        }
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

    private async Task EditTaskEnabled(CatalogTask? catalogTask)
    {
        EditedTask = catalogTask;

        await LoadRelatedTasks();

        Files = (await FileService.GetAllByFilterAsync(new File { TaskId = EditedTask?.Id }) ?? Array.Empty<File>()).ToList();


        if (Files?.ToList() is not null and { Count: > 0 } files)
        {
            foreach (var file in Files)
            {
                var selectedFile = new UploadFileItem
                {
                    Id = file.Id.ToString(),
                    FileName = file.Name,
                    Url = $"https://sandme.ru/file-storage/api/File/GetFile?bucketName=task-files&fileName={file.Name}"
                };

                DefaultFileList?.Add(selectedFile);
            }
        }
        else
        {
            EditedTask!.Files = [];
            DefaultFileList = [];
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
            Files = catalogTask.Files
        };

        _visibleModal = true;

        StateHasChanged();
    }

    private async Task LoadRelatedTasks()
    {
        var mainTaskList = await RelatedTaskService.GetAllByFilterAsync(new RelatedTask { SubordinateTaskId = EditedTask!.Id });
        var subordinateTaskList = await RelatedTaskService.GetAllByFilterAsync(new RelatedTask { MainTaskId = EditedTask!.Id });

        RelatedTasks = [.. mainTaskList, .. subordinateTaskList];
        RelatedTaskIds = [.. mainTaskList.Select(task => task.MainTaskId), .. subordinateTaskList.Select(task => task.SubordinateTaskId)];
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

        DefaultFileList = new();
        _visibleModal = false;

        StateHasChanged();
        return Task.CompletedTask;
    }

    private async Task SaveEditedTask()
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
                IsLoadingTaskList = true;
                StateHasChanged();

                Console.WriteLine($"{EditedTask?.Id} {EditedTask?.Name} is updated.");

                UpdatingDependentAttributes();

                EditedTask!.UserUpdated = CurrentUser;
                EditedTask.DateUpdated = DateTime.Now;

                var updateResult = await CatalogTaskService.UpdateAsync(EditedTask!);
                if (updateResult.Success)
                {
                    Console.WriteLine($"{EditedTask?.Id} {EditedTask?.Name} is updated.");

                    var createdTask = updateResult.Body;

                    await DeleteFiles();

                    foreach (var file in SelectedFiles)
                        await UploadFile(file, createdTask);

                    if (EditedTask?.Files is { Count: > 0 })
                    {

                    }
                    else
                        foreach (var fileEntity in EditedTask?.Files!)
                            await FileService.DeleteAsync(fileEntity.Id);

                    await UpdateRelatedTasks();
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

                var createRet = await CatalogTaskService.CreateSameTaskAsync(CatalogTaskForChangelog, true);
                if (createRet.Success)
                {
                    Console.WriteLine($"{EditedTask?.Id} {EditedTask?.Name} is updated.");
                    if (CatalogTaskForChangelog.Files is { Count: > 0 })
                    {
                        var createdTask = createRet.Body;
                        foreach (var file in SelectedFiles)
                            await UploadFile(file, createdTask);
                    }
                    else
                        foreach (var fileEntity in CatalogTaskForChangelog.Files!)
                            await FileService.DeleteAsync(fileEntity.Id);
                }

                DefaultFileList = new();
                _visibleModal = false;

                CatalogTasks = (await CatalogTaskService.GetAllByPageAsync(new FilterModel { PageNumber = CurrentPage, PageSize = PageSize })).ToList();

                IsLoadingTaskList = false;
                StateHasChanged();
            }
        }
    }

    private async Task UpdateRelatedTasks()
    {
        foreach (var relatedTask in RelatedTasks)
            await RelatedTaskService.DeleteAsync(relatedTask.Id);

        foreach (var relatedTaskId in RelatedTaskIds)
        {
            var relatedTask = new RelatedTask
            {
                MainTaskId = EditedTask!.Id,
                SubordinateTaskId = relatedTaskId
            };

            var result = await RelatedTaskService.CreateAsync(relatedTask);
            Console.WriteLine(result.Success ? $"Related task {relatedTaskId} was added" : result.ErrorMessage);
        }
    }

    private void UpdatingDependentAttributes()
    {
        if (EditedTask == null)
            return;

        if (AuthorsList != null)
        {
            EditedTask.TaskAuthor = AuthorsList.FirstOrDefault(a => a.Id == EditedTask.TaskAuthorId);
            EditedTask.TaskExecutor = AuthorsList.FirstOrDefault(a => a.Id == EditedTask.TaskExecutorId);
        }

        if (InformationSystemList != null)
            EditedTask.InformationSystem = InformationSystemList.FirstOrDefault(a => a.Id == EditedTask.InformationSystemId);
        if (PriorityList != null)
            EditedTask.Priority = PriorityList.FirstOrDefault(a => a.Id == EditedTask.PriorityId);
        if (TaskStatusList != null)
            EditedTask.TaskStatus = TaskStatusList.FirstOrDefault(a => a.Id == EditedTask.TaskStatusId);
    }

    private void DeleteTask(CatalogTask task)
    {
        DeletedTask = task;
        ModalService.Confirm(new ConfirmOptions
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
        IsLoadingTaskList = true;
        StateHasChanged();

        Filter = new CatalogTask
        {
            Id = SelectedTaskId ?? 0,
            InformationSystemId = SelectedInformationSystem?.Id,
            TaskAuthorId = SelectedAuthor?.Id,
            TaskExecutorId = SelectedExecutor?.Id
        };

        var result = (await CatalogTaskService.GetAllByFilterByPageAsync(Filter, CurrentPage, PageSize)).ToList();

        CatalogTasks = new List<CatalogTask>(result);
        CatalogTasksCount = (await CatalogTaskService.GetAllByFilterAsync(Filter)).Count();

        IsLoadingTaskList = false;
        StateHasChanged();
    }


    private async Task HandlePageChange(PaginationEventArgs arg)
    {
        IsLoadingTaskList = true;
        StateHasChanged();

        CurrentPage = arg.Page;
        PageSize = arg.PageSize;

        CatalogTasks = Filter == null ? (await CatalogTaskService.GetAllByPageAsync(new FilterModel { PageSize = PageSize, PageNumber = CurrentPage })).ToList() : (await CatalogTaskService.GetAllByFilterByPageAsync(Filter, CurrentPage, PageSize)).ToList();

        IsLoadingTaskList = false;
        StateHasChanged();
    }

    private async Task Open(CatalogTask task)
    {
        NavigationManager.NavigateTo($"task-read/{task.Id}");
        StateHasChanged();
    }

    private void HandleFileSelected(InputFileChangeEventArgs e)
    {
        SelectedFiles.Clear();
        SelectedFiles.AddRange(e.GetMultipleFiles());
    }

    private async Task UploadFile(IBrowserFile file, CatalogTask task)
    {
        if (file != null)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var formFile = new MultipartFormDataContent();

            var fileName = $"{timestamp}_{task.Name}_{file.Name}";

            formFile.Add(new StreamContent(file.OpenReadStream(524288000)), "file", fileName);

            var createResponse =
                await HttpClient.PostAsync(
                    "https://sandme.ru/file-storage/api/File/UploadFile?bucketName=task-files", formFile);

            if (createResponse.IsSuccessStatusCode)
            {
                File = new File
                {
                    TaskId = task.Id,
                    Name = fileName,
                    DateCreated = DateTime.UtcNow.AddHours(3),
                };

                var result = await FileService.CreateAsync(File);

                if (!result.Success)
                {
                    await HttpClient.PostAsJsonAsync(
                        $"https://sandme.ru/file-storage/api/File/UploadFile?bucketName=task-files&{fileName}", string.Empty);

                    await Message.Error($"Error adding a file {file.Name}");
                }
            }
            else
                await Message.Error("File upload error");
        }
    }

    private async Task DeleteFiles()
    {
        var exceptedPictures = new List<File>();

        var selectedFilesIds = DefaultFileList?.Select(file => file.Id);
        exceptedPictures = Files?.Where(picture => !selectedFilesIds!.Contains(picture.Id.ToString())).ToList();

        if (!exceptedPictures.IsNullOrEmpty())
        {
            foreach (var exceptedPicture in exceptedPictures)
            {
                await HttpClient.PostAsJsonAsync($"https://sandme.ru/file-storage/api/File/DeleteFile?bucketName=task-files&fileName={exceptedPicture.Name}", string.Empty);
                await FileService.DeleteAsync(Convert.ToInt32(exceptedPicture.Id));
            }
        }
    }
}