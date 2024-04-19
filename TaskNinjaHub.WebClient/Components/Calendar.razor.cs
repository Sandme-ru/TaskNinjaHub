using Microsoft.AspNetCore.Components;
using System.Security.Cryptography;
using System.Text;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.CalendarDay.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.TaskStatuses.Enum;
using TaskNinjaHub.WebClient.Services.Bases;
using TaskNinjaHub.WebClient.Services.HttpClientServices;

namespace TaskNinjaHub.WebClient.Components;

public partial class Calendar
{
    #region INJECTIONS

    [Inject] 
    private IUserProviderService UserProviderService { get; set; } = null!;

    [Inject]
    private CatalogTaskService CatalogTaskService { get; set; } = null!;

    [Inject]
    private AuthorService AuthorService { get; set; } = null!;

    #endregion

    #region PROPERTY

    private List<CatalogTask> Tasks { get; set; } = null!;

    private List<Author> Authors { get; set; } = [];

    private List<List<CalendarDay>> CalendarDays { get; set; } = null!;
    
    private int _year = DateTime.Today.Year;

    private int _month = DateTime.Today.Month;

    private const int DaysPeerWeek = 7;

    private const int MonthPeerYear = 12;

    #endregion

    #region METHODS

    private void GenerateCalendar(int year, int month)
    {
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var firstDayOfMonth = new DateTime(year, month, 1);
        var startingDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
        startingDayOfWeek = (startingDayOfWeek == 0) ? DaysPeerWeek - 1 : startingDayOfWeek - 1;

        CalendarDays = [];

        var currentDay = 1;
        for (var i = 0; i < DaysPeerWeek - 1; i++)
        {
            var week = new List<CalendarDay>();
            for (var j = 0; j < DaysPeerWeek; j++)
            {
                if ((i == 0 && j < startingDayOfWeek) || currentDay > daysInMonth)
                    week.Add(new CalendarDay { DayNumber = 0 });
                else
                {
                    var isToday = currentDay == DateTime.Today.Day && year == DateTime.Today.Year && month == DateTime.Today.Month;
                    var currentDate = new DateTime(year, month, currentDay);
                    var tasksForDay = Tasks.Where(task =>
                        task.DateStart.HasValue && task.DateStart.Value.Date <= currentDate &&
                        (!task.DateEnd.HasValue || task.DateEnd.Value.Date >= currentDate)).ToList();

                    if (tasksForDay.Any())
                    {
                        var day = new CalendarDay { DayNumber = currentDay, IsToday = isToday };
                        foreach (var task in tasksForDay)
                        {
                            var taskEndDate = task.DateEnd ?? DateTime.Today;
                            if (taskEndDate > currentDate)
                                taskEndDate = currentDate;

                            var taskColor = GenerateColorFromId(task.Id);

                            day.Tasks.Add(new TaskInfo
                            {
                                TaskId = task.Id,
                                StartDate = task.DateStart!.Value,
                                EndDate = taskEndDate,
                                Color = taskColor
                            });
                        }
                        week.Add(day);
                    }
                    else
                    {
                        week.Add(new CalendarDay { DayNumber = currentDay, IsToday = isToday });
                    }
                    currentDay++;
                }
            }
            CalendarDays.Add(week);
            if (currentDay > daysInMonth)
                break;
        }
    }

    private void PreviousMonth()
    {
        if (_month == 1)
        {
            _year--;
            _month = MonthPeerYear;
        }
        else
            _month--;

        GenerateCalendar(_year, _month);
    }

    private void NextMonth()
    {
        if (_month == MonthPeerYear)
        {
            _year++;
            _month = 1;
        }
        else
            _month++;

        GenerateCalendar(_year, _month);
    }

    private void HandleMonthChange(ChangeEventArgs e)
    {
        _month = int.Parse(e.Value?.ToString()!);
        GenerateCalendar(_year, _month);
    }

    private void GoToSelectedMonthYear(ChangeEventArgs e)
    {
        _year = int.Parse(e.Value?.ToString()!);
        GenerateCalendar(_year, _month);
    }

    private void GoToSelected()
    {
        GenerateCalendar(_year, _month);
    }

    private async Task HandleExecutorChange(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var executorId))
        {
            var selectedAuthor = Authors.FirstOrDefault(author => author.Id == executorId);
            if (selectedAuthor != null)
            {
                await UploadTasks(selectedAuthor.Id);
                
                GenerateCalendar(_year, _month);
                StateHasChanged();
            }
        }
    }
    
    protected override async void OnInitialized()
    {
        await UploadTasks(UserProviderService.User.Id);

        Authors = (await AuthorService.GetAllAsync()).ToList();

        await base.OnInitializedAsync();
        GenerateCalendar(_year, _month);
        StateHasChanged();
    }

    private async Task UploadTasks(int executorId)
    {
        Tasks = (await CatalogTaskService.GetAllByFilterAsync(new CatalogTask { TaskExecutorId = executorId, TaskStatusId = (int)EnumTaskStatus.AtWork })).ToList();
        Tasks.AddRange(await CatalogTaskService.GetAllByFilterAsync(new CatalogTask { TaskExecutorId = executorId, TaskStatusId = (int)EnumTaskStatus.Done }));
        Tasks.AddRange(await CatalogTaskService.GetAllByFilterAsync(new CatalogTask { TaskExecutorId = executorId, TaskStatusId = (int)EnumTaskStatus.AwaitingVerification }));
    }

    private string GenerateColorFromId(int taskId)
    {
        using (var md5 = MD5.Create())
        {
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(taskId.ToString()));
            var sb = new StringBuilder();

            for (var i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("X2"));

            return "#" + sb.ToString().Substring(0, 6);
        }
    }

    #endregion
}