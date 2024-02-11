namespace BO;
/// <summary>
/// task presentation
/// </summary>
/// <param name="Id"></param>Task ID number
/// <param name="NickName"></param>A short, unique name for a task
/// <param name="Description"></param> task description
/// <param name="Status"></param> Task Status

public class TaskInList
{
    public override string ToString() => Tools.ToStringProperty(this);
    public int Id { get; set; }
    public string?  Description { get; set; }
    public string?  NickName { get; set; }
    public Status?  Status { get; set; }

}
