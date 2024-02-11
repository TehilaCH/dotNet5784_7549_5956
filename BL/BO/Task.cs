using System.Net.NetworkInformation;

namespace BO;
/// <summary>
/// task logical entity
/// </summary>
/// <param name="Id"></param>Task ID number
/// <param name="NickName"></param>A short, unique name for a task
/// <param name="Description"></param> task description
/// <param name="CreatTaskDate"></param>indicates the time when the task was created by the administrator
/// <param name="PlannedDateStartWork"></param>a date that is calculated later when creating the schedule,
/// so that according to the available information, all tasks will be performed until the time the project is finished
/// <param name="StartDateTask"></param>When an engineer begins actual work on the task
/// <param name="TimeRequired"></param>The amount of time required to perform the task
/// <param name="Deadline"></param>Latest possible completion date - the latest possible date on which the completion of the task will not cause the project to fail, so that the entire sequence of tasks that depend on it will be completed before the deadline of the entire project.
/// <param name="EndDate"></param>When an engineer reports that he has finished working on the task
/// <param name="Product"></param>A string describing the results or items provided at the end of the task.
/// <param name="Remarks"></param>
/// <param name="Description"></param>List of task dependencies
/// <param name="Status"></param>task status
/// <param name="Engineer"></param>engineer assigned to the task
/// /// <param name="TaskLave"></param> task level
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
