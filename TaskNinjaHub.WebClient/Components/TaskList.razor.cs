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

    private CatalogTask EditedTask { get; set; } = new();

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

    private EditContext EditContext { get; set; } = null!;

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
        NavigationManager.NavigateTo($"/edit-task/{catalogTask!.Id}");

        StateHasChanged();
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