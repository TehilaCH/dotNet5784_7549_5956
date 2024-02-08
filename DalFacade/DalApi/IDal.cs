
using DO;

namespace DalApi;
public interface IDal
{
    IEngineer Engineer { get; }
    IDependence Dependence { get; }
    ITask Task { get; }

    DateTime? StartProjectDate { get; set; }
    DateTime? EndProjectDate { get; set; }
}
