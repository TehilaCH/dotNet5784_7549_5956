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



    public DateTime? StartProjectDate { get =>Instance.StartProjectDate; set => Instance.StartProjectDate=value; }
    public DateTime? EndProjectDate { get => Instance.EndProjectDate; set => Instance.EndProjectDate= value; }

    public void saveDateInFail (string s_data_config_xml, string elemName, DateTime elemValue)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_data_config_xml);
        DateTime? date = root.ToDateTimeNullable(elemName);
        if (date != null) throw new DO.DalAlreadyExistsException($"the DATE is already set to {date}");
        root.Element(elemName)?.SetValue(elemValue);
        XMLTools.SaveListToXMLElement(root, s_data_config_xml);

    }
    
    public DateTime? getStartOrEndDateFromFile (string elemName)
    {
        XElement root =XMLTools.LoadListFromXMLElement("data_config");
        return(root.ToDateTimeNullable(elemName));
    }
}
