using System.Net.NetworkInformation;

namespace BO;
public class Task
{
    public override string ToString() => Tools.ToStringProperty(this);
    public int Id { get; init; }
    public string? Description { get; set; } = null;
    public string? NickName { get; set; } = null;
    public Status? Status { get; set; }=null;
    public List<TaskInList>? Dependencies { get; init; } = null;
    public DateTime? CreatTaskDate { get; set; } = null;
    public TimeSpan? TimeRequired { get; set; } = null;
    public DateTime? PlannedDateStartWork { get; set; } = null;
    public DateTime? StartDateTask { get; set; } = null;
    public DateTime? Deadline { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;
    public string? Product { get; set; } = null;
    public string? Remarks { get; set; } = null;
    public EngineerInTask? Engineer { get; set; } = null;
    public EngineerLevel? TaskLave { get; set; } = null;
    


}
