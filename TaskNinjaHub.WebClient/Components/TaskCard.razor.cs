using AntDesign;
using DiffMatchPatch;
using Microsoft.AspNetCore.Components;
using TaskNinjaHub.Application.Entities.RelatedTasks.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.WebClient.Services;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;

namespace TaskNinjaHub.WebClient.Components;

public partial class TaskCard
{
    [Parameter]
    public int Id { get; set; }

    #region INJECTIONS

    [Inject]
    private FileService FileService { get; set; } = null!;

    [Inject]
    private CatalogTaskService CatalogTaskService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private RelatedTaskService RelatedTaskService { get; set; } = null!;

    #endregion

    #region PROPERTY

    public CatalogTask SelectedCatalogTask { get; set; } = null!;

    public List<CatalogTask> CatalogTasks { get; set; } = null!;

    private List<string> HtmlMarkupForTask { get; set; } = null!;

    private List<UploadFileItem>? DefaultFileList { get; set; } = [];

    private List<CatalogTask>? CatalogTasksForChangelog { get; set; }

    private List<RelatedTask>? MainRelatedTasks { get; set; }

    private List<RelatedTask>? SubordinateRelatedTasks { get; set; }

    private bool IsLoading { get; set; }

    public bool ShowRelatedTasks { get; set; } = true;

    private bool PreviewVisible { get; set; } = false;

    private string PreviewTitle { get; set; } = string.Empty;

    private string ImgUrl { get; set; } = string.Empty;

    #endregion

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            IsLoading = true;
            StateHasChanged();

            SelectedCatalogTask = await CatalogTaskService.GetIdAsync(Id);
            CatalogTasks = (await CatalogTaskService.GetAllAsync()).ToList();

            MainRelatedTasks = (await RelatedTaskService.GetAllByFilterAsync(new RelatedTask { MainTaskId = Id })).ToList();
            SubordinateRelatedTasks = (await RelatedTaskService.GetAllByFilterAsync(new RelatedTask { SubordinateTaskId = Id })).ToList();

            await Open(SelectedCatalogTask);

            IsLoading = false;
            StateHasChanged();
        }
    }

    private async Task Open(CatalogTask catalogTask)
    {
        HtmlMarkupForTask = [];

        var taskFiles = (await FileService.GetAllByFilterAsync(new File { TaskId = Id }) ?? Array.Empty<File>()).ToList();
        if (taskFiles?.ToList() is not null and { Count: > 0 } files)
        {
            catalogTask.Files = files;

            foreach (var file in taskFiles)
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
            DefaultFileList = [];

        CatalogTasksForChangelog = CatalogTasks.Where(t => t.OriginalTaskId == catalogTask.Id).OrderByDescending(t => t.DateCreated).ToList();
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
                    body += $"<p>Edit date: {item.DateCreated}</p>" +
                            $"<p>Editor: {item.UserCreated}</p>";

                    if (prev.Name != item.Name)
                    {
                        header += "<p>Name<p>";
                        body += $"<p>Name: {item.Name}&rarr;{prev.Name}</p>";
                    }
                    else
                        body += $"<p>Name: {item.Name}</p>";

                    if (prev.Description != item.Description)
                    {
                        header += "<p>Description<p>";
                        body += TextCompare(item.Description ?? string.Empty, prev.Description ?? string.Empty);
                    }
                    else
                        body += TextCompare(item.Description ?? string.Empty, prev.Description ?? string.Empty);

                    if (prev.InformationSystemId != item.InformationSystemId)
                    {
                        header += "<p>Information system<p>";
                        body += $"<p>Information system: {item.InformationSystem?.Name}&rarr;{prev.InformationSystem?.Name}</p>";
                    }
                    else
                        body += $"<p>Information system: {item.InformationSystem?.Name}</p>";

                    if (prev.TaskExecutorId != item.TaskExecutorId)
                    {
                        header += "<p>Task executor<p>";
                        body += $"<p>Task executor: {item.TaskExecutor?.Name}&rarr;{prev.TaskExecutor?.Name}</p>";
                    }
                    else
                        body += $"<p>Task executor: {item.TaskExecutor?.Name}</p>";

                    if (prev.PriorityId != item.PriorityId)
                    {
                        header += "<p>Priority</p>";
                        body += $"<p>Priority: {item.Priority?.Name}&rarr;{prev.Priority?.Name}</p>";
                    }
                    else
                        body += $"<p>Priority: {item.Priority?.Name}</p>";

                    if (prev.TaskStatusId != item.TaskStatusId)
                    {
                        header += "<p>Task Status</p>";
                        body += $"<p>Task status: {item.TaskStatus?.Name}&rarr;{prev.TaskStatus?.Name}</p>";
                    }
                    else
                        body += $"<p>Task status: {item.TaskStatus?.Name}</p>";

                    var result = header + "</b>" + body;
                    HtmlMarkupForTask.Add(result);

                    prev = item;
                }
            }
        }
    }

    private string TextCompare(string oldValue, string newValue)
    {
        var dmp = new diff_match_patch();
        List<Diff> diff = dmp.diff_main(oldValue, newValue);
        dmp.diff_cleanupSemantic(diff);

        var result = dmp.diff_prettyHtml(diff);

        return $"<p>Description: {result}</p>";
    }

    private void GoBack()
    {
        NavigationManager.NavigateTo("/catalog-tasks");
    }

    private void NavigateToSubordinateTask(RelatedTask task)
    {
        if (task.SubordinateTask != null)
            NavigationManager.NavigateTo($"task-read/{task.SubordinateTask.Id}", true);
    }

    private void NavigateToMainTask(RelatedTask task)
    {
        if (task.MainTask != null)
            NavigationManager.NavigateTo($"task-read/{task.MainTask.Id}", true);
    }

    private void ToggleRelatedTasks()
    {
        ShowRelatedTasks = !ShowRelatedTasks;
    }
}