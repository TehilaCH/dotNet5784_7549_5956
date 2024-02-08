namespace BlApi;
using BO;

public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }
    public ProjectStatus projectlevel();
}
