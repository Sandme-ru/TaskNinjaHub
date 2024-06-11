using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.RelatedTasks.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Dto;
using TaskNinjaHub.Application.Entities.TaskStatuses.Domain;
using TaskNinjaHub.Application.Entities.TaskStatuses.Enum;
using TaskNinjaHub.Application.Entities.TaskTypes.Domain;
using TaskNinjaHub.Application.Utilities.OperationResults;
using TaskNinjaHub.WebClient.Services.Bases;
using TaskNinjaHub.WebClient.Services.HttpClientServices;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;

namespace TaskNinjaHub.WebClient.Components;

public partial class TaskCreateForm
{
    #region INJECTIONS

    [Inject]
    private IMessageService MessageService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private CatalogTaskService CatalogTaskService { get; set; } = null!;

    [Inject]
    private FileService FileService { get; set; } = null!;

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
    private IUserProviderService UserProviderService { get; set; } = null!;

    [Inject]
    private MinioService MinioService { get; set; } = null!;

    [Inject]
    private MachineLearningService MachineLearningService { get; set; } = null!;

    [Inject]
    private TaskTypeService TaskTypeService { get; set; } = null!;

    [Inject]
    private IMachineLearningModeOptionService MachineLearningModeOptionService { get; set; } = null!;

    #endregion

    #region PROPERTY

    private Author CurrentUser { get; set; } = null!;

    private File File { get; set; } = null!;

    private CatalogTask CreatedTask { get; set; } = new();

    private List<Author> Authors { get; set; } = [];

    private List<Priority> Priorities { get; set; } = [];

    private List<InformationSystem> InformationSystems { get; set; } = [];

    private List<CatalogTaskStatus> TaskStatuses { get; set; } = [];

    private CatalogTaskStatus DefaultStatus { get; set; } = new();

    private bool IsLoading { get; set; }

    private IEnumerable<CatalogTask> CatalogTaskList { get; set; } = [];

    private IEnumerable<int> RelatedTaskId { get; set; } = [];

    private string PredictProbabilityMessage { get; set; } = string.Empty;

    private string PredictProbabilityMessageStyle { get; set; } = string.Empty;

    public IEnumerable<CatalogTaskType> CatalogTaskTypes { get; set; } = [];

    private List<IBrowserFile> _selectedFiles = [];

    private EditContext EditContext { get; set; } = null!;

    #endregion

    #region METHODS

    protected override void OnInitialized()
    {
        CurrentUser = UserProviderService.User;
        EditContext = new EditContext(CreatedTask);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            IsLoading = true;
            StateHasChanged();

            Authors = (await AuthorService.GetAllAsync()).ToList();
            Priorities = (await PriorityService.GetAllAsync()).ToList();
            InformationSystems = (await InformationSystemService.GetAllAsync()).ToList();
            TaskStatuses = (await TaskStatusService.GetAllAsync()).ToList();
            CatalogTaskTypes = await TaskTypeService.GetAllAsync();
            CatalogTaskList = (await CatalogTaskService.GetAllAsync()).Where(task => task.OriginalTaskId == null).ToList();

            DefaultStatus = TaskStatuses.Single(t => t.Id == (int)EnumTaskStatus.AwaitingExecution);
            CreatedTask.TaskStatusId = DefaultStatus.Id;
            
            IsLoading = false;
            StateHasChanged();
        }
    }

    private async Task CreateTask()
    {
        var isValid = EditContext.Validate();

        if (!isValid)
            return;

        IsLoading = true;
        StateHasChanged();
        
        var taskAuthor = await AuthorService.GetAllByFilterAsync(new Author { Id = CurrentUser.Id });
        
        CreatedTask.TaskAuthorId = taskAuthor.FirstOrDefault()!.Id;
        CreatedTask.UserCreated = CurrentUser.Name;
        CreatedTask.DateCreated = DateTime.UtcNow;

        var result = await CatalogTaskService.CreateAsync(CreatedTask);

        if (result.Success)
        {
            //foreach (var file in _selectedFiles)
            //    await UploadFile(file, result.Body);

            await MessageService.Success("The task was successfully added.");

            await AddRelatedTasks(result);

            IsLoading = false;
            StateHasChanged();

            NavigationManager.NavigateTo("/catalog-tasks");
        }
        else
        {
            await MessageService.Error(result.ErrorMessage);

            IsLoading = false;
            StateHasChanged();
        }
    }

    private async Task AddRelatedTasks(OperationResult<CatalogTask> responseMessage)
    {
        if (!RelatedTaskId.IsNullOrEmpty())
        {
            foreach (var relatedTaskId in RelatedTaskId)
            {
                if (relatedTaskId != 0)
                {
                    var relatedTask = new RelatedTask
                    {
                        MainTaskId = responseMessage.Body.Id,
                        SubordinateTaskId = relatedTaskId
                    };

                    await RelatedTaskService.CreateAsync(relatedTask);
                }
            }
        }
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

                //var result = await FileService.CreateAsync(File);

                //if (!result.Success)
                //{
                //    await MinioService.UploadFileByName(fileName);
                //    await MessageService.Error($"Error adding a file {file.Name}");
                //}
            }
            else
                await MessageService.Error("File upload error");
        }
    }

    private async Task PredictProbability()
    {
        if (CreatedTask.TaskExecutorId == 0 ||
            CreatedTask.InformationSystemId == 0 ||
            CreatedTask.PriorityId == 0 ||
            CreatedTask.TaskTypeId == 0)
        {
            PredictProbabilityMessageStyle = "warning";
            PredictProbabilityMessage = "Please fill in all fields before predicting probability.";
            return;
        }

        IsLoading = true;
        StateHasChanged();

        var taskInputDto = new TaskInputDto
        {
            TaskExecutorId = Convert.ToDouble(CreatedTask.TaskExecutorId),
            InformationSystemId = Convert.ToDouble(CreatedTask.InformationSystemId),
            PriorityId = Convert.ToDouble(CreatedTask.PriorityId),
            TaskTypeId = Convert.ToDouble(CreatedTask.TaskTypeId)
        };

        var result = await MachineLearningService.PredictProbability(taskInputDto);
        if (result is { Success: true })
        {
            PredictProbabilityMessageStyle = result.Body switch
            {
                > 0.75 => "success",
                > 0.5 => "warning",
                _ => "danger"
            };

            PredictProbabilityMessage = $"There is a {result.Body * 100}% chance that the performer is suitable for this task";
        }
        else
            await MessageService.Error(result?.ErrorMessage);

        IsLoading = false;
        StateHasChanged();
    }

    #endregion
}