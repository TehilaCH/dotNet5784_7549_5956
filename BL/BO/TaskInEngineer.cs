namespace BO;
/// <summary>
/// A class that represents a task assigned to an engineer with two fields: id and name of an engineer
/// </summary>
public class TaskInEngineer
{
    public override string ToString() => Tools.ToStringProperty(this);
    public int? Id { get; set; }
    public string? NickName { get; set; }
}
