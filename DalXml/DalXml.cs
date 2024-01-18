using DalApi;
namespace Dal;

//stage 3
sealed public class DalXml : IDal
{

public IEngineer Engineer => new EngineerImplementation();

    public IDependence Dependence => new DependenceImplementation();

    public ITask Task => new TaskImplementation();
}
