
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int newTaskId = DataSource.Config.NextTaskId;
        item.IdTask = newTaskId; //Update field of the automatic identification number to the next value
        DataSource.Tasks.Add(item); // Create the entity
        return newTaskId;

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {

        foreach (var task in DataSource.Tasks)
        {
            if (task.IdTask == id)
            {
                DataSource.Tasks.Remove(task);
                return;
            }
        }
        throw new Exception($"Task with ID={id} already exists");
       
    }

    public Task? Read(int id)
    {

        foreach (var task in DataSource.Tasks)
        {
            if (task.TaskId == id)
            {
                return task;
            }
        }
        return null;
       
    }

    public List<Task> ReadAll()
    {

        List<Task> copyTasks = new List<Task>();

        foreach (var task in DataSource.Tasks)
        {
            Task copyTask1 = new Task
            {
                TaskId = task.TaskId,
                Nickname = task.Nickname,
                Milestone = task.Milestone,
                Description = task.Description,
                CreateTaskDate = task.CreateTaskDate,
                PlannedDateStartWork = task.PlannedDateStartWork,
                StartDateTask = task.StartDateTask,
                TimeRequired = task.TimeRequired,
                Deadline = task.Deadline,
                EndDate = task.EndDate,
                Product = task.Product,
                commentary = task.commentary,
                EngineerIdToTask = task.EngineerIdToTask,
                TaskLave = task.TaskLave
            };

            copyTasks.Add(copyTask1);
        }

        return copyTasks;


    }

    public void Update(Task item)
    {
        foreach (var task in DataSource.Tasks)
        {
            if (task.IdTask == item.IdTask)
            {
                DataSource.Tasks.Remove(task);
                DataSource.Tasks.Add(item);
                return;
            }
        }

        throw new Exception($"Task with ID={item.IdTask} already exists");
        
    }
}

   

   
  

