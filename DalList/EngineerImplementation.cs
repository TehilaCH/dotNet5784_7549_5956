﻿
namespace Dal;
using DalApi;
using DO;
using System.Collections;
using System.Collections.Generic;
//using System.Xml.Linq;

public class EngineerImplementation : IEngineer
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
            if (engineer.IdNum == item.IdNum)
            {
                throw new Exception($"Engineer with ID={item.IdNum} already exists");
            }
        }
        DataSource.Engineers.Add(item);

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
            if(engineer.IdNum == id)
            {
                DataSource.Engineers.Remove(engineer);
                return;
            }
        }
            throw new Exception($"Engineer with ID={id} already exists");
    }
    /// <summary>
    /// Returns the object if it exists otherwise returns null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Engineer? Read(int id) 
    {
        foreach (var engineer in DataSource .Engineers)
        {
            if(engineer.IdNum==id)
            {
                return engineer;    
            }
        }
       return null; 
    }
    /// <summary>
    /// Making a copy of the existing list of all objects of type T Returning the copy
    /// </summary>
    /// <returns></returns>
    public List<Engineer> ReadAll()
    {
        List<Engineer> CopyEngineers = new List<Engineer>();
        foreach (var engineer in DataSource.Engineers)
        {
            CopyEngineers.Add(new Engineer
            {
                IdNum = engineer.IdNum,
                Name = engineer.Name,
                Email = engineer.Email,
                EngineerLevel = engineer.EngineerLevel,
                CostPerHour = engineer.CostPerHour


            });
        }

        return CopyEngineers;
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
            if(engineer.IdNum==item.IdNum)
            {
                DataSource.Engineers.Remove(engineer);
                DataSource.Engineers.Add(item);
                return;
            }
        }

        throw new Exception($"Engineer with ID={item.IdNum} already exists");
    }
}
