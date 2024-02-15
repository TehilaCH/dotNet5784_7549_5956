namespace Dal;
using DalApi;
using System;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class ScheduleImplementation : ISchedule
{
    private readonly string _dataConfigXml = "data-config";
    public DateTime? getEndProjectDate()
    {
        XElement root = XMLTools.LoadListFromXMLElement(_dataConfigXml).Element("EndProjectDate")!;
        if (root.Value == "")
            return null;
        return DateTime.Parse(root.Value);
        
    }

    public DateTime? getStartProjectDate()
    {
        XElement root = XMLTools.LoadListFromXMLElement(_dataConfigXml).Element("StartProjectDate")!;
        if (root.Value == "")
            return null;
        return DateTime.Parse(root.Value);

    }

    public void resetTime()
    {
        XElement root = XMLTools.LoadListFromXMLElement(_dataConfigXml);
        root.Element("StartProjectDate")!.Value = " ";
        root.Element("EndProjectDate")!.Value = " ";
        XMLTools.SaveListToXMLElement(root, _dataConfigXml);
    }

    public DateTime? SetEndProjectDate(DateTime date)
    {
        XElement root = XMLTools.LoadListFromXMLElement(_dataConfigXml);
        root.Element("EndProjectDate")!.Value= date.ToString();
        XMLTools.SaveListToXMLElement(root, _dataConfigXml);
        return date;
    }

    public DateTime? SetStartProjectDate(DateTime date)
    {
        XElement root = XMLTools.LoadListFromXMLElement(_dataConfigXml);
        root.Element("StartProjectDate")!.Value = date.ToString();
        XMLTools.SaveListToXMLElement(root, _dataConfigXml);
        return date;
    }
}
