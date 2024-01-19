

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
        XElement engineerElem = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        if (engineerElem.Elements("Engineer").Any(e => e.ToIntNullable("Id") == item.IdNum))
        {
            throw new DalXMLFileLoadCreateException($"An Engineer with IdNum {item.IdNum} already exists.");
        }

        XElement itemElement = new XElement("Engineer",
            new XElement("Id", item.IdNum),
            new XElement("Name", item.Name),
            new XElement("Email", item.Email),
            new XElement("CostPerHour", item.CostPerHour),
            new XElement("EngineerLevel", item.EngineerLevel)
        );

        engineerElem.Add(itemElement);
        XMLTools.SaveListToXMLElement(engineerElem, s_engineers_xml);
        return item.IdNum;
    }
    /// <summary>
    /// Deleting an entity from the fail XML if exists otherwise throw exception
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Delete(int id)
    {
        XElement engineerElem = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        XElement engineerToRemove = engineerElem.Elements("Engineer").FirstOrDefault(e => e.ToIntNullable("Id") == id);

        if (engineerToRemove != null)
        {
            engineerToRemove.Remove();
            XMLTools.SaveListToXMLElement(engineerElem, s_engineers_xml);
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
        return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(s => getEngineer(s)).FirstOrDefault(filter);
      
    }
    /// <summary>
    ///  Returns the object from the fail XML if exists otherwise returns null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Engineer? Read(int id)
    {
       
        XElement engineerElem = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        XElement? engineerElement = engineerElem.Elements("Engineer").FirstOrDefault(st => (int?)st.Element("Id") == id);
        return engineerElement is null ? null : getEngineer(engineerElement);
    }
    /// <summary>
    /// Making a copy of the existing list of all objects of type E Returning the copy
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        if (filter == null)
            return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(s => getEngineer(s));
        else
            return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(s => getEngineer(s)).Where(filter);

    }
    /// <summary>
    /// Updating an entity in the XML fail if exists delete and add the new one
    /// if not exist throw exception​
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalXMLFileLoadCreateException"></exception>
    public void Update(Engineer item)
    {
        XElement engineerElem = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        XElement engineerToUpdate = engineerElem.Elements("Engineer").FirstOrDefault(e => e.ToIntNullable("Id") == item.IdNum);
        if (engineerToUpdate != null)
        {
            engineerToUpdate.Remove();
            XElement itemElement = new XElement("Engineer",
            new XElement("Id", item.IdNum),
            new XElement("Name", item.Name),
            new XElement("Email", item.Email),
            new XElement("CostPerHour", item.CostPerHour),
            new XElement("EngineerLevel", item.EngineerLevel)
        );
            engineerElem.Add(itemElement);
            XMLTools.SaveListToXMLElement(engineerElem, s_engineers_xml);
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
        XElement engineerElem = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        engineerElem.RemoveAll();
        XMLTools.SaveListToXMLElement(engineerElem, s_engineers_xml);

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    static Engineer getEngineer (XElement s)
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


