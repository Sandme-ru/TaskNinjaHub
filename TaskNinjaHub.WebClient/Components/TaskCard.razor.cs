using AntDesign;
using DiffMatchPatch;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.WebClient.Services;

namespace TaskNinjaHub.WebClient.Components;

public partial class TaskCard
{
    [Parameter]
    public int Id { get; set; }
    
    [Inject]
    private FileService FileService { get; set; } = null!;

    [Inject]
    private CatalogTaskService CatalogTaskService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    public CatalogTask SelectedCatalogTask { get; set; } = null!;
    
    public List<CatalogTask> CatalogTasks { get; set; } = null!;
    
    private List<string> HtmlMarkupForTask { get; set; } = null!;

    private List<UploadFileItem>? DefaultFileList { get; set; } = [];

    private List<CatalogTask>? CatalogTasksForChangelog { get; set; }

    private bool IsPreviewVisible { get; set; }

    private string? FilePreviewUrl { get; set; } = string.Empty;

    private string? FilePreviewTitle { get; set; } = string.Empty;
    
    private void OnPreview(UploadFileItem file)
    {
        if (!file.IsPicture())
        {
            NavigationManager.NavigateTo(file.ObjectURL, true);
            return;
        }

        IsPreviewVisible = true;
        FilePreviewTitle = file.FileName;
        FilePreviewUrl = file.ObjectURL;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            SelectedCatalogTask = await CatalogTaskService.GetIdAsync(Id);
            CatalogTasks = (await CatalogTaskService.GetAllAsync()).ToList();

            await Open(SelectedCatalogTask);
        }
    }

    private async Task Open(CatalogTask catalogTask)
    {
        HtmlMarkupForTask = [];

        var taskFiles = await FileService.GetAllByTaskIdAsync(catalogTask!.Id);
        if (taskFiles?.ToList() is not null and { Count: > 0 } files)
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
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                    State = UploadState.Success
                }).ToList()!;
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
                        body += TextCompare(item.Description!, prev.Description!);
                    }
                    else
                        body += TextCompare(item.Description!, prev.Description!);

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

        StateHasChanged();
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
        NavigationManager.NavigateTo("");
    }
}