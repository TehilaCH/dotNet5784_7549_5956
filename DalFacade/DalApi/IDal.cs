
namespace DalApi;
public interface IDal
{
    IEngineer Engineer { get; }
    IDependence Dependence { get; }
    ITask Task { get; }
}
