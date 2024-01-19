
namespace Dal;

using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// class implements the CRUD functions for the Dependence entity
/// </summary>
internal class DependenceImplementation : IDependence
{
    readonly string s_dependences_xml = "dependences";//field that is the database of that entity.

    /// <summary>
    /// The function creates a new entity in the fail XML if not exist and throws an exception if it exists​
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Dependence item)
    {
        List<Dependence> dependences = XMLTools.LoadListFromXMLSerializer<Dependence>(s_dependences_xml);
        int nextId = Config.NextDependenceId;
        Dependence dependence = item with {IdNum = nextId };
        dependences.Add(dependence);
        XMLTools.SaveListToXMLSerializer(dependences, s_dependences_xml);
        return nextId;
    }
    /// <summary>
    /// Deleting an entity from the fail XML if exists otherwise throw exception
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalXMLFileLoadCreateException"></exception>
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
    /// <summary>
    /// method that returns an object not only by ID but by another parameter from the XML fail.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Dependence? Read(Func<Dependence, bool> filter)
    {
        List<Dependence> dependences = XMLTools.LoadListFromXMLSerializer<Dependence>(s_dependences_xml);
        return dependences.FirstOrDefault(filter);
    }
    /// <summary>
    /// Returns the object from the fail XML if exists otherwise returns null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Making a copy of the existing list of all objects of type E Returning the copy
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Updating an entity in the XML fail if exists delete and add the new one
    /// if not exist throw exception​
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalXMLFileLoadCreateException"></exception>
    public void Update(Dependence item)
    {
        List<Dependence> dependences = XMLTools.LoadListFromXMLSerializer<Dependence>(s_dependences_xml);
        if (dependences.RemoveAll(it => it.IdNum == item.IdNum) == 0)
            throw new DalXMLFileLoadCreateException($"Task with ID={item.IdNum} does not exsist");
        dependences.Add(item);
        XMLTools.SaveListToXMLSerializer(dependences, s_dependences_xml);
    }
    /// <summary>
    /// clear the XML fail
    /// </summary>
    public void clear() 
    {
        List<Dependence> dependences = XMLTools.LoadListFromXMLSerializer<Dependence>(s_dependences_xml);
        dependences.Clear();
        XMLTools.SaveListToXMLSerializer(dependences, s_dependences_xml);
    }
}
