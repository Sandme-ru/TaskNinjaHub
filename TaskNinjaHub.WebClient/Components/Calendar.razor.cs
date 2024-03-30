using Microsoft.AspNetCore.Components;
using TaskNinjaHub.Application.Entities.CalendarDay.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.TaskStatuses.Enum;
using TaskNinjaHub.WebClient.Services;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Components;

public partial class Calendar
{
    [Inject] 
    private IUserProviderService UserProviderService { get; set; } = null!;

    [Inject]
    private CatalogTaskService CatalogTaskService { get; set; } = null!;

    private IEnumerable<CatalogTask> CatalogTasks { get; set; } = null!;

    private int _year = DateTime.Today.Year;

    private int _month = DateTime.Today.Month;

    private List<List<CalendarDay>> CalendarDays { get; set; } = null!;

    private void GenerateCalendar(int year, int month)
    {
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var firstDayOfMonth = new DateTime(year, month, 1);
        var startingDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
        startingDayOfWeek = (startingDayOfWeek == 0) ? 6 : startingDayOfWeek - 1;

        CalendarDays = new List<List<CalendarDay>>();

        var currentDay = 1;
        for (var i = 0; i < 6; i++)
        {
            var week = new List<CalendarDay>();
            for (var j = 0; j < 7; j++)
            {
                if ((i == 0 && j < startingDayOfWeek) || currentDay > daysInMonth)
                    week.Add(new CalendarDay { DayNumber = 0 });
                else
                {
                    var isToday = currentDay == DateTime.Today.Day && year == DateTime.Today.Year && month == DateTime.Today.Month;
                    var tasksForDay = CatalogTasks.Where(task => task.DateCreated.Value.Day == currentDay && task.DateCreated.Value.Month == month && task.DateCreated.Value.Year == year).ToList();
                    if (tasksForDay.Any())
                    {
                        var day = new CalendarDay { DayNumber = currentDay, IsToday = isToday };
                        foreach (var task in tasksForDay)
                        {
                            day.Tasks.Add(new TaskInfo {TaskId = task.Id });
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
            _month = 12;
        }
        else
            _month--;

        GenerateCalendar(_year, _month);
    }

    private void NextMonth()
    {
        if (_month == 12)
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

    protected override async void OnInitialized()
    {
        CatalogTasks = await CatalogTaskService.GetAllByFilterAsync(new CatalogTask { TaskExecutorId = UserProviderService.User.Id, TaskStatusId = (int)EnumTaskStatus.AtWork });

        await base.OnInitializedAsync();
        GenerateCalendar(_year, _month);
        StateHasChanged();
    }
}