
using DO;

namespace DalApi;
public interface IDal
{
    IEngineer Engineer { get; }
    IDependence Dependence { get; }
    ITask Task { get; }
    public void saveDateInFail(string _dataConfigXml, string elemName, DateTime elemValue);

    public DateTime? getStartDateFromFile(string elemName);
}
