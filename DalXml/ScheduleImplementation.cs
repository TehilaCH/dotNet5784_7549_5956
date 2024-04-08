namespace Dal;
using DalApi;
using System;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class ScheduleImplementation : ISchedule
{
    private readonly string _dataConfigXml = "data-config";

    public DateTime? getEndProjectDate()//Returns project end date
    {
        XElement root = XMLTools.LoadListFromXMLElement(_dataConfigXml).Element("EndProjectDate")!;
        if (root.Value == "")
            return null;
        return DateTime.Parse(root.Value);//Converts to a date

    }

    public DateTime? getStartProjectDate()//Returns project start date
    {
        XElement root = XMLTools.LoadListFromXMLElement(_dataConfigXml).Element("StartProjectDate")!;
        if (root.Value == "")
            return null;
        return DateTime.Parse(root.Value);//Converts to a date

    }

    public void resetTime()//Resets project dates
    {
        XElement root = XMLTools.LoadListFromXMLElement(_dataConfigXml);
        root.Element("StartProjectDate")!.Value = " ";//Resets - puts an empty value
        root.Element("EndProjectDate")!.Value = " ";//Resets - puts an empty value
        XMLTools.SaveListToXMLElement(root, _dataConfigXml);//Saves changes
    }

    public DateTime? SetEndProjectDate(DateTime date)//puts Project start date
    {
        XElement root = XMLTools.LoadListFromXMLElement(_dataConfigXml);
        root.Element("EndProjectDate")!.Value= date.ToString();//Puts the date and converts to a string
        XMLTools.SaveListToXMLElement(root, _dataConfigXml);//Saves data
        return date;
    }

    public DateTime? SetStartProjectDate(DateTime date)//puts Project end date
    {
        XElement root = XMLTools.LoadListFromXMLElement(_dataConfigXml);
        root.Element("StartProjectDate")!.Value = date.ToString();//Puts the date and converts to a string
        XMLTools.SaveListToXMLElement(root, _dataConfigXml);//Saves data
        return date;
    }
    public void resetRunNumber()//Resets running numbers
    {
        XElement root = XMLTools.LoadListFromXMLElement(_dataConfigXml);
        root.Element("NextTaskId")!.Value = "1";
        root.Element("NextDependenceId")!.Value = "1";
        XMLTools.SaveListToXMLElement(root, _dataConfigXml);
    }
}
