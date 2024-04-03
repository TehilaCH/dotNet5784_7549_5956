

namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;//

/// <summary>
/// class implements the CRUD functions for the Engineer entity
/// </summary>
internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";
    /// <summary>
    /// The function creates a new entity in the fail XML if not exist and throws an exception if it exists​
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="DalXMLFileLoadCreateException"></exception>
    public int Create(Engineer item)
    {
        XElement engineerElem = XMLTools.LoadListFromXMLElement(s_engineers_xml);//Moves the file to the object
        if (engineerElem.Elements("Engineer").Any(e => e.ToIntNullable("Id") == item.IdNum))//Checking if something like this exists
        {
            throw new DalXMLFileLoadCreateException($"An Engineer with IdNum {item.IdNum} already exists.");
        }

        XElement itemElement = new XElement("Engineer",  //Creates an engineer
            new XElement("Id", item.IdNum),
            new XElement("Name", item.Name),
            new XElement("Email", item.Email),
            new XElement("CostPerHour", item.CostPerHour),
            new XElement("EngineerLevel", item.EngineerLevel)
        );

        engineerElem.Add(itemElement);//adding
        XMLTools.SaveListToXMLElement(engineerElem, s_engineers_xml);//Save changes
        return item.IdNum;
    }
    /// <summary>
    /// Deleting an entity from the fail XML if exists otherwise throw exception
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Delete(int id)
    {
        XElement engineerElem = XMLTools.LoadListFromXMLElement(s_engineers_xml);//Passes data to an object
        XElement engineerToRemove = engineerElem.Elements("Engineer").FirstOrDefault(e => e.ToIntNullable("Id") == id)!;//Checks if the engineer is present

        if (engineerToRemove != null)//if exists
        {
            engineerToRemove.Remove();//Deletes the engineer
            XMLTools.SaveListToXMLElement(engineerElem, s_engineers_xml);//Save changes
        }
        else
        {
            throw new InvalidOperationException($"No Engineer with IdNum {id} was found.");
        }
    }
    /// <summary>
    /// method that returns an object not only by ID but by another parameter from the XML fail.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(s => getEngineer(s)).FirstOrDefault(filter);//Returns if the filter is met

    }
    /// <summary>
    ///  Returns the object from the fail XML if exists otherwise returns null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Engineer? Read(int id)
    {
       
        XElement engineerElem = XMLTools.LoadListFromXMLElement(s_engineers_xml);//Passes data to an object
        XElement? engineerElement = engineerElem.Elements("Engineer").FirstOrDefault(st => (int?)st.Element("Id") == id);//Checks if the engineer is available
        return engineerElement is null ? null : getEngineer(engineerElement);//If present returns the object otherwise null
    }
    /// <summary>
    /// Making a copy of the existing list of all objects of type E Returning the copy
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        if (filter == null)
            return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(s => getEngineer(s));//Returns a copy of a list of engineers who perform the filtering
        else
            return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(s => getEngineer(s)).Where(filter);//Returns a copy of a list of engineers

    }
    /// <summary>
    /// Updating an entity in the XML fail if exists delete and add the new one
    /// if not exist throw exception​
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalXMLFileLoadCreateException"></exception>
    public void Update(Engineer item)
    {
        XElement engineerElem = XMLTools.LoadListFromXMLElement(s_engineers_xml);//Passes data to an object
        XElement engineerToUpdate = engineerElem.Elements("Engineer").FirstOrDefault(e => e.ToIntNullable("Id") == item.IdNum)!;//Checks if the engineer is present
        if (engineerToUpdate != null)//if exists
        {
            engineerToUpdate.Remove();//delete engineer
            XElement itemElement = new XElement("Engineer",//Creates an engineer
            new XElement("Id", item.IdNum),
            new XElement("Name", item.Name),
            new XElement("Email", item.Email),
            new XElement("CostPerHour", item.CostPerHour),
            new XElement("EngineerLevel", item.EngineerLevel)
        );
            engineerElem.Add(itemElement);//Updating a new engineer
            XMLTools.SaveListToXMLElement(engineerElem, s_engineers_xml);//Save changes
        }

        else
        {
            throw new DalXMLFileLoadCreateException($"No Engineer with IdNum {item.IdNum} was found.");
        }
    }
    /// <summary>
    /// clear the XML fail
    /// </summary>
    public void clear() 
    {
        XElement engineerElem = XMLTools.LoadListFromXMLElement(s_engineers_xml);//Passes data to an object
        engineerElem.RemoveAll();//Deletes data
        XMLTools.SaveListToXMLElement(engineerElem, s_engineers_xml);//Save changes

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    static Engineer getEngineer (XElement s)//Creates an engineer object and return
    {
        return new Engineer()
        {
            IdNum = s.ToIntNullable("Id") ?? throw new FormatException("can't convert id"),
            Name = (string?)s.Element("Name") ?? "",
            Email = (string?)s.Element("Email") ?? "",
            CostPerHour = s.ToDoubleNullable("CostPerHour") ?? null,
            EngineerLevel = s.ToEnumNullable<EngineerLevel>("EngineerLevel") ?? null,
        };
    }
}


