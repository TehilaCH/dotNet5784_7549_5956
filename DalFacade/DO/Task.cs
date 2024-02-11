

namespace DO;

/// <summary>
/// Task planning, stages (deadlines - start, end/estimated), assigning a task to an engineer, product
/// </summary>
/// <param name="TaskId"></param>Task ID number
/// <param name="Nickname"></param>A short, unique name for a task
/// <param name="EngineerNumIdToTask"></param>
/// <param name="Description"></param> task description
/// <param name="Milestone"></param> Boolean ranking, in the first three stages, the DAL layer, its value will be False in all tasks. Later a task entity will be able to show a milestone and then its value can also be true.​
/// <param name="CreatTaskDate"></param>indicates the time when the task was created by the administrator
/// <param name="PlannedDateStartWork"></param>a date that is calculated later when creating the schedule, so that according to the available information, all tasks will be performed until the time the project is finished
/// <param name="StartDateTask"></param>When an engineer begins actual work on the task
/// <param name="TimeRequired"></param>The amount of time required to perform the task
/// <param name="Deadline"></param>Latest possible completion date - the latest possible date on which the completion of the task will not cause the project to fail, so that the entire sequence of tasks that depend on it will be completed before the deadline of the entire project.
/// <param name="EndDate"></param>When an engineer reports that he has finished working on the task
/// <param name="Product"></param>A string describing the results or items provided at the end of the task.
/// <param name="Remarks"></param>
///  <param name="TaskLave"></param>task level
public record Task
(
   int TaskId, // key (run)
   int? EngineerIdToTask,
   string? Nickname=null,
   string? Description = null,
   bool Milestone = false,
   DateTime? CreatTaskDate = null,
   DateTime? PlannedDateStartWork = null,
   DateTime? StartDateTask = null,
   TimeSpan? TimeRequired = null,
   DateTime? Deadline = null,
   DateTime? EndDate = null,
   string? Product = null,
   string? Remarks = null,
   DO.EngineerLevel? TaskLave = null


 )
{

    public Task() : this(0,0) { } //empty ctor 
    
}





