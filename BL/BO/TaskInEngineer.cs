namespace BO;

public class TaskInEngineer
{
    public override string ToString() => Tools.ToStringProperty(this);
    public int? Id { get; set; }
    public string? NickName { get; set; }
}
