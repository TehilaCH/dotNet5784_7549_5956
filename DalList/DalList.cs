
namespace Dal;
using DalApi;
using DO;
using System.Reflection.Metadata.Ecma335;

sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }
    public IEngineer Engineer => new EngineerImplementation();

    public IDependence Dependence => new DependenceImplementation();

    public ITask Task => new TaskImplementation();

    public ISchedule Schedule =>  new ScheduleImplementation();
}
