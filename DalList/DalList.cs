
namespace Dal;
using DalApi;
using DO;
using System.Reflection.Metadata.Ecma335;

sealed internal class DalList : IDal//typhus internal and sealed for the singleton
{
    public static IDal Instance { get; } = new DalList();//field for the singleton
    private DalList() { }//An empty private constructor for the singleton
    public IEngineer Engineer => new EngineerImplementation();

    public IDependence Dependence => new DependenceImplementation();

    public ITask Task => new TaskImplementation();

    public ISchedule Schedule =>  new ScheduleImplementation();
}
