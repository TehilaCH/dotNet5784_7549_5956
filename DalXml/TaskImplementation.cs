

namespace Dal;
using DalApi;
using DO;
using System.Data.Common;


internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";
    /// <summary>
    /// 
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

    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        return tasks.FirstOrDefault(filter);
    }

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

    public void Update(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        if(tasks.RemoveAll(it=> it.TaskId==item.TaskId)==0)
            throw new DalXMLFileLoadCreateException($"Task with ID={item.TaskId} does not exsist");
        tasks.Add(item);
        XMLTools.SaveListToXMLSerializer(tasks,s_tasks_xml);
    }
}


