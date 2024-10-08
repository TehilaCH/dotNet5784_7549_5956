﻿
namespace Dal;
using DalApi;
using DO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//using System.Xml.Linq;

internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// The function creates a new entity if it does not exist and throws an exception if it exists​
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int Create(Engineer item)
    {

        foreach (var engineer in DataSource.Engineers) //check if item exists and throws an exception if it exists
        {
            if (engineer.IdNum == item.IdNum)//Checks if the engineer is exist throws an exception
            {
                throw new DalAlreadyExistsException($"Engineer with ID={item.IdNum} already exists");
            }
        }
        DataSource.Engineers.Add(item);//Adds an engineer to the list

        return item.IdNum;
    }

    /// <summary>
    /// Deleting an entity if it exists otherwise an exception is thrown
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {
        foreach (var engineer in DataSource.Engineers)
        {
            if(engineer.IdNum == id)//Checks if a Engineer exists
            {
                DataSource.Engineers.Remove(engineer);//Deletes the Engineer
                return;
            }
        }
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exists");//Throws an exception
                                                                                         //if the Engineer does not exist

    }
    /// <summary>
    /// Returns the object if it exists otherwise returns null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Engineer? Read(int id)
    {
        return (from engineer in DataSource.Engineers
               where engineer.IdNum == id
               select engineer).FirstOrDefault();
   
    }
    /// <summary>
    /// method that returns an object not only by ID but by another parameter.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Engineer? Read(Func<Engineer, bool> filter) 
    { 
        return DataSource.Engineers.FirstOrDefault(filter);
    }

    /// <summary>
    /// Making a copy of the existing list of all objects of type T Returning the copy
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null) //stage 2
    {
        if (filter != null)//If there is filtering
        {
            return from item in DataSource.Engineers
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Engineers //If there is no filtering
               select item;
    }
    /// <summary>
    /// Updating an entity if it exists we will delete it and add the new one
    /// and if it doesn't exist we will throw an exception​
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public void Update(Engineer item)
    {
        foreach(var engineer in DataSource .Engineers)
        {
            if(engineer.IdNum==item.IdNum)//Checks if exists
            {
                DataSource.Engineers.Remove(engineer);//Delete old engineer 
                DataSource.Engineers.Add(item);//Adds an updated engineer
                return;
            }
        }

        throw new DalDoesNotExistException($"Engineer with ID={item.IdNum} does not exists");
    }
    /// <summary>
    /// clear the list of Engineers
    /// </summary>
    public void clear()
    {
        DataSource.Engineers.Clear();
    }
}
