using System.Net.NetworkInformation;

namespace BO;
public class Task
{
    public int Id { get; init; }
    public string? Description { get; set; }
    public string? NickName { get; set; }
    public Status? Status { get; set; }
    public List<TaskInList>? Dependencies { get; init; } = null;
    public DateTime? CreatTaskDate { get; set; }
    public TimeSpan? TimeRequired { get; set; }
    public DateTime? PlannedDateStartWork { get; set; }
    public DateTime? StartDateTask { get; set; }
    public DateTime? Deadline { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Product { get; set; }
    public string? Remarks { get; set; }
    public EngineerInTask? Engineer { get; set;}
    public EngineerLevel? TaskLave { get; set; }
    


}
