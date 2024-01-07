
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class DependenceImplementation : IDependence
{
    public int Create(Dependence item)//create Dependence
    {

        int DependenceId = DataSource.Config.NextDependenceId;//creates automatic ID
        Dependence copt= item with { IdNum = DependenceId };
        DataSource.Dependences.Add(copt); //add Dependence to the Dependence list
        return DependenceId;

    }

    public void Delete(int id)//Delete Dependences with id that receives
    {
        foreach (var dependence in DataSource.Dependences)
        {
            if (dependence.IdNum == id)
            {
                DataSource.Dependences.Remove(dependence);
                return;
            }
        }
        throw new Exception($"Dependence with ID={id} already exists");
        
    }

    public Dependence? Read(int id)//search Dependence and return if ut found else return null
    {
        foreach (var dependence in DataSource.Dependences)
        {
            if (dependence.IdNum == id)
            {
                return dependence;
            }
        }
        return null;
        
    }

    public List<Dependence> ReadAll()//creates a copy of a list Dependences and return it
    {
        List<Dependence> CopyDependences = new List<Dependence>();
        foreach (var dependence in DataSource.Dependences)
        {
            CopyDependences.Add(new Dependence
            {
                IdNum = dependence.IdNum,
                IdPendingTask= dependence.IdPendingTask,
                IdPreviousTask= dependence.IdPreviousTask,
            });
        }

        return CopyDependences;
       

    }

    public void Update(Dependence item)//Update Dependence if exists
    {
        foreach (var task in DataSource.Dependences)
        {
            if (task.IdNum == item.IdNum)
            {
                DataSource.Dependences.Remove(task);
                DataSource.Dependences.Add(item);
                return;
            }
        }

        throw new Exception($"Dependence with ID={item.IdNum} already exists");
        
    }
}