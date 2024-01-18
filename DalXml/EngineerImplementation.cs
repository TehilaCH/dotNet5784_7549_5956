

namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;

internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";

    public int Create(Engineer item)
    {
        XElement engineerElem = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        if (engineerElem.Elements("Engineer").Any(e => e.ToIntNullable("Id") == item.IdNum))
        {
            throw new InvalidOperationException($"An Engineer with IdNum {item.IdNum} already exists.");
        }
        engineerElem.Add(item);
        XMLTools.SaveListToXMLElement(engineerElem, s_engineers_xml);
        return item.IdNum;
    }

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

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(s => getEngineer(s)).FirstOrDefault(filter);
      
    }

    public Engineer? Read(int id)
    {
        XElement ? engineerElem = XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().FirstOrDefault(st => (int?)st.Element("id") == id);
        return engineerElem is null ? null : getEngineer(engineerElem);
       

    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        if (filter == null)
            return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(s => getEngineer(s));
        else
            return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(s => getEngineer(s)).Where(filter);

        
    }

    public void Update(Engineer item)
    {
        XElement engineerElem = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        XElement engineerToUpdate = engineerElem.Elements("Engineer").FirstOrDefault(e => e.ToIntNullable("Id") == item.IdNum);
        if (engineerToUpdate != null)
        {
            engineerToUpdate.Remove();
            engineerElem.Add(item);
            XMLTools.SaveListToXMLElement(engineerElem, s_engineers_xml);
        }

        else
        {
            throw new InvalidOperationException($"No Engineer with IdNum {item.IdNum} was found.");
        }
    }

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


