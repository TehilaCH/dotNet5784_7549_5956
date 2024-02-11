namespace BO;
/// <summary>
/// A class that indicates which engineer is assigned to a 
/// task has two fields: Id engineer and name​ engineer
/// </summary>
public class EngineerInTask
{
    public override string ToString() => Tools.ToStringProperty(this);
    public int? Id { get; set; }
    public string? Name { get; set; }


}
