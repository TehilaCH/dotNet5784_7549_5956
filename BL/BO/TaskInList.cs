namespace BO;

public class TaskInList
{
    public override string ToString() => Tools.ToStringProperty(this);
    public int Id { get; set; }
    public string?  Description { get; set; }
    public string?  NickName { get; set; }
    public Status?  Status { get; set; }

}
