
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
/// <summary>
/// The function creates a new entity if it does not exist and throws an exception if it exists​
/// </summary>
internal class DependenceImplementation : IDependence
{
    public int Create(Dependence item)//create Dependence
    {

        int DependenceId = DataSource.Config.NextDependenceId;//creates automatic ID
        Dependence copt= item with { IdNum = DependenceId };
        DataSource.Dependences.Add(copt); //add Dependence to the Dependence list
        return DependenceId;

    }
    /// <summary>
    /// Delete Dependences with id that receives
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        foreach (var dependence in DataSource.Dependences)
        {
            if (dependence.IdNum == id)//Checks if a dependency exists
            {
                DataSource.Dependences.Remove(dependence);//Deletes the dependency
                return;
            }
        }
        throw new DalDoesNotExistException($"Dependence with ID={id} does not exists");//Throws an exception
                                                                                       //if the dependency does not exist

    }
    /// <summary>
    /// search Dependence and return if ut found else return null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Dependence? Read(int id)
    {
        return (from dependence in DataSource.Dependences
                where dependence.IdNum == id
                select dependence).FirstOrDefault();

    }

    /// <summary>
    /// method that returns an object not only by ID but by another parameter.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Dependence? Read(Func<Dependence, bool> filter)
    {
        return DataSource.Dependences.FirstOrDefault(filter);
    }

    /// <summary>
    /// creates a copy of a list Dependences and return it
    /// </summary>
    /// <returns></returns>

    public IEnumerable<Dependence> ReadAll(Func<Dependence, bool>? filter = null) 
    {
        if (filter != null) //If there is filtering
        {
            return from item in DataSource.Dependences
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Dependences //If there is no filtering
               select item;
    }
    /// <summary>
    /// Updating an entity if it exists we will delete it and add the new one
    /// and if it doesn't exist we will throw an exception​
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public void Update(Dependence item)
    {
        foreach (var task in DataSource.Dependences)
        {
            if (task.IdNum == item.IdNum)//Checks if the dependency exists
            {
                DataSource.Dependences.Remove(task);//Deletes the old entity
                DataSource.Dependences.Add(item);//Added updated entity
                return;
            }
        }

        throw new DalDoesNotExistException($"Dependence with ID={item.IdNum} does not exists");//Throws an exception
                                                                                               //if the dependency does not exist

    }
    /// <summary>
    /// clear the list of Dependences
    /// </summary>
    public void clear() 
    {
        DataSource.Dependences.Clear();
    }
}