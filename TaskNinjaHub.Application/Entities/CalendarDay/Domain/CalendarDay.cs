namespace TaskNinjaHub.Application.Entities.CalendarDay.Domain;

public class CalendarDay
{
    public int DayNumber { get; set; }

    public bool IsToday { get; set; }

    public List<TaskInfo> Tasks { get; set; } = new();
}

public class TaskInfo
{
    public int TaskId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
