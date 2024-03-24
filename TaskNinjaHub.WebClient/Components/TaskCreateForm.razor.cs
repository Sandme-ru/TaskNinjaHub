using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using System.Text.Json;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.RelatedTasks.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.TaskStatuses.Domain;
using TaskNinjaHub.Application.Utilities.OperationResults;
using TaskNinjaHub.WebClient.Services;
using TaskNinjaHub.WebClient.Services.Bases;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;

namespace TaskNinjaHub.WebClient.Components;

public partial class TaskCreateForm
{
    #region INJECTIONS

    [Inject]
    private IMessageService Message { get; set; } = null!;

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

    #endregion

    #region PROPERTY

    private bool IsPreviewVisible { get; set; }

    private string? FilePreviewUrl { get; set; } = string.Empty;

    private string? FilePreviewTitle { get; set; } = string.Empty;

    private Author CurrentUser { get; set; } = null!;

    private File File { get; set; } = null!;

    private CatalogTask CreatedCatalogTask { get; set; } = new();

    private List<Author> AuthorsList { get; set; } = [];

    private List<Priority> PriorityList { get; set; } = [];

    private List<InformationSystem> InformationSystemList { get; set; } = [];

    private List<File> UploadedFileList { get; set; } = [];

    private List<CatalogTaskStatus> TaskStatusList { get; set; } = [];

    private CatalogTaskStatus? DefaultStatus { get; set; } = new();

    public bool IsLoading { get; set; }

    public IEnumerable<CatalogTask> CatalogTaskList { get; set; } = [];

    public IEnumerable<int> RelatedTaskId { get; set; }

    private List<IBrowserFile> SelectedFiles = new List<IBrowserFile>();

    private HttpClient HttpClient { get; set; } = new();

    #endregion

    protected override void OnInitialized()
    {
        CurrentUser = UserProviderService.User;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            IsLoading = true;
            StateHasChanged();

            AuthorsList = (await AuthorService.GetAllAsync()).ToList();
            PriorityList = (await PriorityService.GetAllAsync()).ToList();
            InformationSystemList = (await InformationSystemService.GetAllAsync()).ToList();
            TaskStatusList = (await TaskStatusService.GetAllAsync()).ToList();
            CatalogTaskList = (await CatalogTaskService.GetAllAsync()).Where(task => task.OriginalTaskId == null).ToList();

            DefaultStatus = TaskStatusList.FirstOrDefault(t => t.Id == 1);

            IsLoading = false;
            StateHasChanged();
        }
    }

    private async Task CreateTask()
    {
        IsLoading = true;
        StateHasChanged();

        CreatedCatalogTask.TaskAuthorId =
            (await AuthorService.GetAllByFilterAsync(new Author { AuthGuid = CurrentUser.AuthGuid }))
            .FirstOrDefault()
            ?.Id;

        //CreatedCatalogTask.TaskAuthorId = CurrentUser.Id;
        CreatedCatalogTask.TaskStatusId = DefaultStatus?.Id;
        CreatedCatalogTask.UserCreated = CurrentUser.Name;
        CreatedCatalogTask.DateCreated = DateTime.UtcNow;

        var result = await CatalogTaskService.CreateAsync(CreatedCatalogTask);

        if (result.Success)
        {
            foreach (var file in SelectedFiles)
                await UploadFile(file, result.Body);

            await Message.Success("The task was successfully added.");

            await AddRelatedTasks(result);

            IsLoading = false;
            StateHasChanged();

            NavigationManager.NavigateTo("/catalog-tasks");
        }
        else
        {
            await Message.Error(result.ErrorMessage);

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

    private void OnFinish()
    {
        Console.WriteLine($"Success:{JsonSerializer.Serialize(CreatedCatalogTask)}");
    }

    private void OnFinishFailed()
    {
        Console.WriteLine($"Failed:{JsonSerializer.Serialize(CreatedCatalogTask)}");
    }

    // TODO: ПОДУМАТЬ НАД РЕМУВОМ

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
}