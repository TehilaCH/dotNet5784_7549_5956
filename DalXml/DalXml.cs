using DalApi;
using DO;
using System.Xml.Linq;
namespace Dal;
sealed internal class DalXml : IDal//typhus internal and sealed for the singleton
{
    public static IDal Instance { get; } = new DalXml();    //field for the singleton
    private DalXml() { } //An empty private constructor for the singleton

    public IEngineer Engineer => new EngineerImplementation();

    public IDependence Dependence => new DependenceImplementation();

    public ITask Task => new TaskImplementation();

    public ISchedule Schedule => new ScheduleImplementation();
}
