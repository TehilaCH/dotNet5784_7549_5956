
namespace Dal;
using DalApi;
using DO;

sealed public class DalList : IDal
{
    public IEngineer Engineer => new EngineerImplementation();

    public IDependence Dependence =>  new DependenceImplementation();

    public ITask Task =>  new TaskImplementation();
}
