
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


}


