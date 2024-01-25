using DalApi;
namespace Dal;
sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();    
    private DalXml() { }

    public IEngineer Engineer => new EngineerImplementation();

    public IDependence Dependence => new DependenceImplementation();

    public ITask Task => new TaskImplementation();
}
