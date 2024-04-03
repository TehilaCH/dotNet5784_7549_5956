

namespace Dal;
using DalApi;
using DO;
using System.Data.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

/// <summary>
/// class implements the CRUD functions for the Task entity
/// </summary>
internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";
    /// <summary>
    /// The function creates a new entity in the fail XML if it does not exist and throws an exception if it exists​
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);//Transfers the data to the list
        int nextId = Config.NextTaskId;
        Task task = item with {TaskId = nextId };//Copies the object except the id
        tasks.Add(task);//Adds to the list
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);//Saves changes to the file
        return nextId;
    }
    /// <summary>
    /// Deleting an entity from the fail XML if exists otherwise throw exception
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalXMLFileLoadCreateException"></exception>
    public void Delete(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);// Transfers data to a list
        foreach (var t in tasks)
        {
            if (t.TaskId == id)//Checks if it exists
            {
                tasks.Remove(t);//Deletes if a task exists
                XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);//save in file
                return;
            }
        }
       
        throw new DalXMLFileLoadCreateException($"Task with ID={id} does not exsist");
      
    }
    /// <summary>
    ///  method that returns an object not only by ID but by another parameter from the XML fail.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);//Transfers data to a list
        return tasks.FirstOrDefault(filter);//Returns if exsist and maintains the filter otherwise null
    }
    /// <summary>
    /// Returns the object from the fail XML if it exists otherwise returns null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>    
    public Task? Read(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        foreach (var t in tasks)
        {
            if (t.TaskId == id)//Returns if exists otherwise null
            {
                return t;
            }
        }
        return null;

    }
    /// <summary>
    /// Making a copy of the existing list of all objects of type T Returning the copy
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);//Transfers the data to the list
        if (filter != null)//With a filter returns the maintainer
        {
            return from item in tasks
                   where filter(item)
                   select item;
        }
        return from item in tasks//No filter returns a copy of a list
               select item;
       
    }
    /// <summary>
    /// Updating an entity in the XML fail if it exists we will delete it and add the new one
    /// and if it doesn't exist we will throw an exception​
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalXMLFileLoadCreateException"></exception>
    public void Update(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);//Transfers data to a list
        if (tasks.RemoveAll(it=> it.TaskId==item.TaskId)==0)//deleted if exsist
            throw new DalXMLFileLoadCreateException($"Task with ID={item.TaskId} does not exsist");
        tasks.Add(item);//deleted if exsist
        XMLTools.SaveListToXMLSerializer(tasks,s_tasks_xml);//Saves the list to a file
    }
    /// <summary>
    /// clear the XML fail
    /// </summary>
    public void clear()
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        tasks.Clear();//Deletes data
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);//Saves an empty list
    }
}


