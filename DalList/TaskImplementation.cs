﻿
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

internal class TaskImplementation : ITask
{/// <summary>
/// The function creates a new entity if it does not exist and throws an exception if it exists​
/// </summary>
/// <param name="item"></param>
/// <returns></returns>
    public int Create(Task item)
    {
        int newTaskId = DataSource.Config.NextTaskId;//creates automatic ID
        Task temp=item with { TaskId=newTaskId };//Copying an entity with the id automatic
        DataSource.Tasks.Add(temp); //Adds the task to the list
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
            if (task.TaskId == id)//Checks if the task exists
            {
                DataSource.Tasks.Remove(task);//Deletes the task
                return;
            }
        }
        throw new DalDoesNotExistException($"Task with ID={id} does not exists");//Throws an exception if a
                                                                                 //task does not exist

    }
    /// <summary>
    /// Returns the object if it exists otherwise returns null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task? Read(int id)
    {

        return (from task in DataSource.Tasks
                where task.TaskId == id
                select task).FirstOrDefault();

    }
    /// <summary>
    /// method that returns an object not only by ID but by another parameter.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Task? Read(Func<Task, bool> filter)
    {
        return DataSource.Tasks.FirstOrDefault(filter);
    }

    /// <summary>
    /// /Making a copy of the existing list of all objects of type T Returning the copy
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null) 
    {
        if (filter != null)//If there is filtering
        {
            return from item in DataSource.Tasks
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Tasks//If there is no filtering
               select item;
    }

    /// <summary>
    /// Updating an entity if it exists we will delete it and add the new one
    /// and if it doesn't exist we will throw an exception​
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public void Update(Task item)
    
    {
        foreach (var task in DataSource.Tasks)
        {
            if (task.TaskId == item.TaskId)//Checks if the task exists
            {
                DataSource.Tasks.Remove(task);//Deletes an old task
                DataSource.Tasks.Add(item);//Adds an updated task
                return;
            }
        }

        throw new DalDoesNotExistException($"Task with ID={item.TaskId} does not exists");//Throws an exception if a
                                                                                          //task does not exist


    }
    /// <summary>
    /// clear the list of Tasks
    /// </summary>
    public void clear()
    {
        DataSource.Tasks.Clear();
    }
}

   

   
  

