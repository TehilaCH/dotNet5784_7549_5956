namespace BlApi;
using BO;

public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }

    public DateTime? StartProjectDate { get; set; }
    public DateTime? EndProjectDate { get; set; }

}
