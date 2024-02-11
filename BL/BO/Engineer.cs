
namespace BO;
/// <summary>
/// A logical entity of an engineer
/// </summary>
/// <param name="IdNum"></param>Engineer ID number
/// <param name="Name"></param>Name of engineer
/// <param name="Email"></param> Engineer's email address
/// <param name="Cost"></param>Cost Per Hour
/// <param name="EngineerLevel"></param>The engineer's experience
/// <param name="Task"></param>task is assigned to an engineer

public class Engineer
{
   public override string ToString() => Tools.ToStringProperty(this);

    public int Id { get; init; }
    public string? Name { get; set; } = null;
    public string? Email { get; set; }=null;
    public EngineerLevel? Level { get; set; } = null;
    public double? Cost { get; set; } = null;
    public TaskInEngineer? Task { get; set; } = null;


}
