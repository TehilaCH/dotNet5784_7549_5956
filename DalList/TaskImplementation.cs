
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

public class TaskImplementation : ITask
{
    public int Create(Task item)//creat item and throws an exception if it exists
    {
        int newTaskId = DataSource.Config.NextTaskId;
        Task temp=item with { TaskId=newTaskId };
        DataSource.Tasks.Add(temp); 
        return newTaskId;
    }
    /// <summary>
    /// Deleting an entity if it exists otherwise an exception is thrown
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {

        foreach (var task in DataSource.Tasks)
        {
            if (task.TaskId == id)
            {
                DataSource.Tasks.Remove(task);
                return;
            }
        }
        throw new Exception($"Task with ID={id} already exists");
       
    }

    public Task? Read(int id)//Returns the object if it exists otherwise returns null
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

    public List<Task> ReadAll()//Making a copy of the existing list of all objects of type T Returning the copy
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
                CreatTaskDate = task.CreatTaskDate,
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

    public void Update(Task item)//Updating an entity if it exists we will delete it and add the new one
    // and if it doesn't exist we will throw an exception​
    {
        foreach (var task in DataSource.Tasks)
        {
            if (task.TaskId == item.TaskId)
            {
                DataSource.Tasks.Remove(task);
                DataSource.Tasks.Add(item);
                return;
            }
        }

        throw new Exception($"Task with ID={item.TaskId} already exists");
        
    }
}

   

   
  

