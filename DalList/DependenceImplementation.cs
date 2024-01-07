
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class DependenceImplementation : IDependence
{
    public int Create(Dependence item)
    {
        /*int nextDependenceId = DataSource.Config.NextDependenceId;
        Dependence newDependence = new Dependence
        {
            IdNum = nextDependenceId,
            //הספתי את שני השורות הבאות כשהייתי במיין 
            IdPendingTask=item.IdPendingTask,
            IdPreviousTask=item.IdPreviousTask,
        };

        DataSource.Dependences.Add(newDependence); // הוספת הישות לרשימה
        return nextDependenceId;*/


        int newTaskId = DataSource.Config.NextTaskId;
        item.IdNum = newTaskId; //Update field of the automatic identification number to the next value
        DataSource.Dependences.Add(item); // Create the entity
        return newTaskId;

    }

    public void Delete(int id)
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

    public Dependence? Read(int id)
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

    public List<Dependence> ReadAll()
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

    public void Update(Dependence item)
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