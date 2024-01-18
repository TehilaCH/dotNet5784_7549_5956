
namespace Dal;

using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;

internal class DependenceImplementation : IDependence
{
    readonly string s_dependences_xml = "dependences";

    public int Create(Dependence item)
    {
        List<Dependence> dependences = XMLTools.LoadListFromXMLSerializer<Dependence>(s_dependences_xml);
        int nextId = Config.NextDependenceId;
        Dependence dependence = item with {IdNum = nextId };
        dependences.Add(dependence);
        XMLTools.SaveListToXMLSerializer(dependences, s_dependences_xml);
        return nextId;
    }

    public void Delete(int id)
    {
        List<Dependence> dependences = XMLTools.LoadListFromXMLSerializer<Dependence>(s_dependences_xml);
        foreach (var d in dependences)
        {
            if (d.IdNum == id)
            {
                dependences.Remove(d);
                XMLTools.SaveListToXMLSerializer(dependences, s_dependences_xml);
                return;
            }
        }

        throw new DalXMLFileLoadCreateException($"Task with ID={id} does not exsist");
    }

    public Dependence? Read(Func<Dependence, bool> filter)
    {
        List<Dependence> dependences = XMLTools.LoadListFromXMLSerializer<Dependence>(s_dependences_xml);
        return dependences.FirstOrDefault(filter);
    }

    public Dependence? Read(int id)
    {
        List<Dependence> dependences = XMLTools.LoadListFromXMLSerializer<Dependence>(s_dependences_xml);
        foreach (var d in dependences)
        {
            if (d.IdNum == id)
            {
                return d;
            }
        }
        return null;
    }

    public IEnumerable<Dependence?> ReadAll(Func<Dependence, bool>? filter = null)
    {
        List<Dependence> dependences = XMLTools.LoadListFromXMLSerializer<Dependence>(s_dependences_xml);
        if (filter != null)
        {
            return from item in dependences
                   where filter(item)
                   select item;
        }
        return from item in dependences
               select item;
    }

    public void Update(Dependence item)
    {
        List<Dependence> dependences = XMLTools.LoadListFromXMLSerializer<Dependence>(s_dependences_xml);
        if (dependences.RemoveAll(it => it.IdNum == item.IdNum) == 0)
            throw new DalXMLFileLoadCreateException($"Task with ID={item.IdNum} does not exsist");
        dependences.Add(item);
        XMLTools.SaveListToXMLSerializer(dependences, s_dependences_xml);
    }
}
