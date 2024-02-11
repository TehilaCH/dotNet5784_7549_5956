
namespace BO;
/// <summary>
/// 
/// </summary>
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
