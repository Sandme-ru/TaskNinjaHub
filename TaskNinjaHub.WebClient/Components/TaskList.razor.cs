﻿using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.RelatedTasks.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.TaskStatuses.Domain;
using TaskNinjaHub.Application.Entities.TaskTypes.Domain;
using TaskNinjaHub.Application.Filters;
using TaskNinjaHub.WebClient.Services.Bases;
using TaskNinjaHub.WebClient.Services.HttpClientServices;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;

namespace TaskNinjaHub.WebClient.Components;

public partial class TaskList
{
    #region INJECTIONS

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

    [Inject]
    private MinioService MinioService { get; set; } = null!;

    [Inject]
    private MachineLearningService MachineLearningService { get; set; } = null!;

    [Inject]
    private TaskTypeService TaskTypeService { get; set; } = null!;

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

    private CatalogTask EditedTask { get; set; }

    private CatalogTask CloneTask { get; set; }

    private CatalogTask? CatalogTaskForChangelog { get; set; }

    private List<CatalogTask>? CatalogTasks { get; set; }

    private List<UploadFileItem>? DefaultFileList { get; set; } = [];

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

    private bool _visibleModal;

    private IEnumerable<Author>? Authors { get; set; }

    private IEnumerable<Priority>? Priorities { get; set; }

    private IEnumerable<InformationSystem>? InformationSystems { get; set; }

    private IEnumerable<CatalogTaskStatus>? TaskStatuses { get; set; }

    private static CatalogTask? DeletedTask { get; set; }

    private Func<ModalClosingEventArgs, Task> _onOk;

    private InformationSystem SelectedInformationSystem { get; set; } = new();

    private Author SelectedAuthor { get; set; } = new();

    private Author SelectedExecutor { get; set; } = new();

    private File File { get; set; } = null!;

    private List<File> Files { get; set; } = [];

    private int? SelectedTaskId { get; set; }

    private int PageSize { get; set; } = 10;

    private int CurrentPage { get; set; } = 1;

    private int CatalogTasksCount { get; set; }

    private bool IsLoadingTaskList { get; set; }

    private CatalogTask Filter { get; set; } = null!;

    public IEnumerable<int> RelatedTaskIds { get; set; } = [];

    public IEnumerable<RelatedTask> RelatedTasks { get; set; } = [];

    public IEnumerable<CatalogTask> Tasks { get; set; } = [];

    private List<IBrowserFile> _selectedFiles = [];
    
    private bool PreviewVisible { get; set; }

    private string PreviewTitle { get; set; } = string.Empty;

    private string ImgUrl { get; set; } = string.Empty;

    public IEnumerable<CatalogTaskType> TaskTypes { get; set; } = [];

    #endregion

    #region METHODS

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            IsLoadingTaskList = true;
            StateHasChanged();

            CatalogTasksCount = await CatalogTaskService.GetAllCountAsync();

            Authors = await AuthorService.GetAllAsync();
            Priorities = await PriorityService.GetAllAsync();
            InformationSystems = await InformationSystemService.GetAllAsync();
            TaskStatuses = await TaskStatusService.GetAllAsync();
            TaskTypes = await TaskTypeService.GetAllAsync();
            CatalogTasks = (await CatalogTaskService.GetAllByPageAsync(new FilterModel { PageNumber = CurrentPage, PageSize = PageSize })).ToList();
            Tasks = (await CatalogTaskService.GetAllAsync()).Where(task => task.OriginalTaskId == null).ToList();

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

        NavigationManager.NavigateTo("/catalog-tasks", true);
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

        Files = (await FileService.GetAllByFilterAsync(new File { TaskId = EditedTask?.Id })).ToList();
        
        if (Files.ToList() is not null and { Count: > 0 } files)
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

        if (catalogTask != null)
            CloneTask = new CatalogTask
            {
                Name = catalogTask.Name,
                Description = catalogTask.Description,
                TaskAuthorId = catalogTask.TaskAuthorId,
                TaskExecutorId = catalogTask.TaskExecutorId,
                InformationSystemId = catalogTask.InformationSystemId,
                PriorityId = catalogTask.PriorityId,
                TaskStatusId = catalogTask.TaskStatusId,
                TaskTypeId = catalogTask.TaskTypeId,
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
            EditedTask.Name = CloneTask.Name;
            EditedTask.Description = CloneTask.Description;
            EditedTask.TaskAuthorId = CloneTask.TaskAuthorId;
            EditedTask.TaskExecutorId = CloneTask.TaskExecutorId;
            EditedTask.InformationSystemId = CloneTask.InformationSystemId;
            EditedTask.PriorityId = CloneTask.PriorityId;
            EditedTask.TaskStatusId = CloneTask.TaskStatusId;
            EditedTask.TaskTypeId = CloneTask.TaskTypeId;
        }

        DefaultFileList = [];
        _visibleModal = false;

        StateHasChanged();
        return Task.CompletedTask;
    }

    private async Task SaveEditedTask()
    {
        if (EditedTask != null)
        {
            if (CheckCloneTask())
                await MessageService.Error("Change one or more attributes");
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

                    foreach (var file in _selectedFiles)
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
                    Name = CloneTask.Name,
                    Description = CloneTask.Description,
                    TaskAuthorId = CloneTask.TaskAuthorId,
                    TaskExecutorId = CloneTask.TaskExecutorId,
                    InformationSystemId = CloneTask.InformationSystemId,
                    PriorityId = CloneTask.PriorityId,
                    TaskStatusId = CloneTask.TaskStatusId,
                    TaskTypeId = CloneTask.TaskTypeId,
                    OriginalTaskId = EditedTask.Id,
                    DateCreated = EditedTask.DateUpdated,
                    UserCreated = EditedTask.UserUpdated,
                    Files = EditedTask.Files
                };

                var createRet = await CatalogTaskService.CreateChangelogTaskAsync(CatalogTaskForChangelog, true);
                if (createRet.Success)
                {
                    Console.WriteLine($"{EditedTask.Id} {EditedTask.Name} is updated.");
                    if (CatalogTaskForChangelog.Files is { Count: > 0 })
                    {
                        var createdTask = createRet.Body;
                        foreach (var file in _selectedFiles)
                            await UploadFile(file, createdTask);
                    }
                    else
                        foreach (var fileEntity in CatalogTaskForChangelog.Files!)
                            await FileService.DeleteAsync(fileEntity.Id);
                }

                DefaultFileList = [];
                _visibleModal = false;

                CatalogTasks = (await CatalogTaskService.GetAllByPageAsync(new FilterModel { PageNumber = CurrentPage, PageSize = PageSize })).ToList();

                IsLoadingTaskList = false;
                StateHasChanged();
            }
        }
    }

    private bool CheckCloneTask()
    {
        return EditedTask?.Name == CloneTask?.Name
               && EditedTask?.Description == CloneTask?.Description
               && EditedTask?.TaskAuthorId == CloneTask?.TaskAuthorId
               && EditedTask?.TaskExecutorId == CloneTask?.TaskExecutorId
               && EditedTask?.InformationSystemId == CloneTask?.InformationSystemId
               && EditedTask?.PriorityId == CloneTask?.PriorityId
               && EditedTask?.TaskStatusId == CloneTask?.TaskStatusId
               && EditedTask?.TaskTypeId == CloneTask?.TaskTypeId
               && Equals(EditedTask?.Files?.ToArray(), DefaultFileList?.ToArray());
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

        if (Authors != null)
        {
            EditedTask.TaskAuthor = Authors.FirstOrDefault(a => a.Id == EditedTask.TaskAuthorId);
            EditedTask.TaskExecutor = Authors.FirstOrDefault(a => a.Id == EditedTask.TaskExecutorId);
        }

        if (InformationSystems != null)
            EditedTask.InformationSystem = InformationSystems.FirstOrDefault(a => a.Id == EditedTask.InformationSystemId);
        if (Priorities != null)
            EditedTask.Priority = Priorities.FirstOrDefault(a => a.Id == EditedTask.PriorityId);
        if (TaskStatuses != null)
            EditedTask.TaskStatus = TaskStatuses.FirstOrDefault(a => a.Id == EditedTask.TaskStatusId);
        if (TaskStatuses != null)
            EditedTask.TaskType = TaskTypes.FirstOrDefault(a => a.Id == EditedTask.TaskTypeId);
    }

    private void DeleteTask(CatalogTask task)
    {
        DeletedTask = task;
    }

    private async Task FilterSelectionChanged()
    {
        IsLoadingTaskList = true;
        StateHasChanged();

        var selectedInformationSystem = 0;
        var selectedAuthor = 0;
        var selectedExecutor = 0;

        if(SelectedInformationSystem != null)
            selectedInformationSystem = SelectedInformationSystem.Id;

        if (SelectedAuthor != null)
            selectedAuthor = SelectedAuthor.Id;

        if (SelectedExecutor != null)
            selectedExecutor = SelectedExecutor.Id;

        Filter = new CatalogTask
        {
            Id = SelectedTaskId ?? 0,
            InformationSystemId = selectedInformationSystem,
            TaskAuthorId = selectedAuthor,
            TaskExecutorId = selectedExecutor
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
        _selectedFiles.Clear();
        _selectedFiles.AddRange(e.GetMultipleFiles());
    }

    private async Task UploadFile(IBrowserFile file, CatalogTask task)
    {
        if (file != null)
        {
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var formFile = new MultipartFormDataContent();

            var fileName = $"{timestamp}_{task.Name}_{file.Name}";

            formFile.Add(new StreamContent(file.OpenReadStream(524288000)), "file", fileName);

            var createResponse = await MinioService.UploadFile(formFile);

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
                    await MinioService.UploadFileByName(fileName);

                    await MessageService.Error($"Error adding a file {file.Name}");
                }
            }
            else
                await MessageService.Error("File upload error");
        }
    }

    private async Task DeleteFiles()
    {
        var selectedFilesIds = DefaultFileList?.Select(file => file.Id);
        var exceptedPictures = Files.Where(picture => !selectedFilesIds!.Contains(picture.Id.ToString())).ToList();

        if (!exceptedPictures.IsNullOrEmpty())
        {
            foreach (var exceptedPicture in exceptedPictures)
            {
                if (exceptedPicture.Name != null) 
                    await MinioService.DeleteFile(exceptedPicture.Name);

                await FileService.DeleteAsync(Convert.ToInt32(exceptedPicture.Id));
            }
        }
    }

    private async Task TrainModel()
    {
        IsLoadingTaskList = true;
        StateHasChanged();

        await MessageService.Info("Training model started");
        var result = await MachineLearningService.TrainTasksModel();

        if (result!.Success)
            await MessageService.Success("Training model finished");
        else
            await MessageService.Error(result.ErrorMessage);

        IsLoadingTaskList = false;
        StateHasChanged();
    }

    #endregion
}