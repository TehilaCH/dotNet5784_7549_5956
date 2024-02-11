
namespace BlImplementation;
using BlApi;
using BO;
using System.Xml.Linq;
using Dal;

internal class Bl : IBl
{
  
    public IEngineer Engineer =>  new EngineerImplementation();

    public ITask Task => new TaskImplementation();
   
    
    public ProjectStatus projectlevel()
    {
        XElement root = XMLTools.LoadListFromXMLElement("data_config");
        DateTime? date = root.ToDateTimeNullable("StartProjectDate");
        if (date == null)
            return ProjectStatus.planingStage;
        else return ProjectStatus.executionStage;

    }

    private string _dataConfigXml = "data_config.xml";
    public void SetStartProjectDate(DateTime date)
    {

        DalApi.Factory.Get. saveDateInFail(_dataConfigXml,"StartProjectDate", date);
    }

    public DateTime? getStartProjectDate()
    {
       
        return getStartDateFromFile("StartProjectDate");
    }

    //public void saveDateInFail(string _dataConfigXml, string elemName, DateTime elemValue)
    //{
    //    XElement root = XMLTools.LoadListFromXMLElement(_dataConfigXml);
    //    DateTime? date = root.ToDateTimeNullable(elemName);
    //    if (date != null) throw new BO.BlAlreadyExistsException($"the DATE is already set to {date}");
    //    root.Element(elemName)?.SetValue(elemValue);
    //    XMLTools.SaveListToXMLElement(root, _dataConfigXml);

    //}
    //public DateTime? getStartDateFromFile(string elemName)
    //{
    //    XElement root = XMLTools.LoadListFromXMLElement(_dataConfigXml);
    //    return (root.ToDateTimeNullable(elemName));
    //}



}



