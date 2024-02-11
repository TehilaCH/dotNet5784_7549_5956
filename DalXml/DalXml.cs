using DalApi;
using DO;
using System.Xml.Linq;
namespace Dal;
sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();    
    private DalXml() { }

    public IEngineer Engineer => new EngineerImplementation();

    public IDependence Dependence => new DependenceImplementation();

    public ITask Task => new TaskImplementation();

    private string _dataConfigXml = "data_config.xml";
    public void saveDateInFail(string _dataConfigXml, string elemName, DateTime elemValue)
    {
        XElement root = XMLTools.LoadListFromXMLElement(_dataConfigXml);
        DateTime? date = root.ToDateTimeNullable(elemName);
        if (date != null) throw new DO.DalAlreadyExistsException($"the DATE is already set to {date}");
        root.Element(elemName)?.SetValue(elemValue);
        XMLTools.SaveListToXMLElement(root, _dataConfigXml);

    }
    public DateTime? getStartDateFromFile(string elemName)
    {
        XElement root = XMLTools.LoadListFromXMLElement(_dataConfigXml);
        return (root.ToDateTimeNullable(elemName));
    }
}
