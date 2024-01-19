

namespace Dal;
using DalApi;
using DO;
using System.Data.Common;

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
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        int nextId = Config.NextTaskId;
        Task task = item with {TaskId = nextId };
        tasks.Add(task);
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
        return nextId;
    }
    /// <summary>
    /// Deleting an entity from the fail XML if exists otherwise throw exception
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalXMLFileLoadCreateException"></exception>
    public void Delete(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        foreach (var t in tasks)
        {
            if (t.TaskId == id)
            {
                tasks.Remove(t);
                XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
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
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        return tasks.FirstOrDefault(filter);
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
            if (t.TaskId == id)
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
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        if (filter != null)
        {
            return from item in tasks
                   where filter(item)
                   select item;
        }
        return from item in tasks
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
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        if(tasks.RemoveAll(it=> it.TaskId==item.TaskId)==0)
            throw new DalXMLFileLoadCreateException($"Task with ID={item.TaskId} does not exsist");
        tasks.Add(item);
        XMLTools.SaveListToXMLSerializer(tasks,s_tasks_xml);
    }
    /// <summary>
    /// clear the XML fail
    /// </summary>
    public void clear()
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        tasks.Clear();
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
    }
}


