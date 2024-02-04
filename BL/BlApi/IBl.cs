using BO;

namespace BlApi;

public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }

    public DateTime? StartProjectDate { get; set; }
    public DateTime? EndProjectDate { get; set; }

    public ProjectStatus projectlevel(DO.Task doTask);
}
