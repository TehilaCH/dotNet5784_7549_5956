
namespace BlImplementation;
using BlApi;
using BO;
using System.Xml.Linq;
using Dal;
using DO;
using System;
using DalApi;
using System.Threading.Tasks;

internal class Bl : IBl
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public BlApi.IEngineer Engineer =>  new EngineerImplementation();

    public BlApi.ITask Task => new TaskImplementation(this);

    public BlApi.ISchedule Schedule => new ScheduleImplementation();

    private static DateTime s_Clock = DateTime.Now.Date;//current Date
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }

    public void AdvanceDay()//Increases days by 1
    {
        s_Clock = s_Clock.AddDays(1);
    }

    public void AdvanceHour()//Increases hours by 1
    {
        s_Clock = s_Clock.AddHours(1);
    }

    public void AdvanceYear()//Increases years by 1
    {
        s_Clock = s_Clock.AddYears(1);
    }
    public void InitializeTime()//current Date
    {
        s_Clock = DateTime.Now;
    }

    public void depAdd(int prev, int dep)//Add dependencies
    {
        if(prev== dep)
        {
            throw new BlInvalidValueException("It is impossible to make a task dependent on itself ");
        }
        DateTime? date = Schedule.getStartProjectDate();
        if (date != null)//Execution stage
        {
            throw new BlInvalidValueException("The Task data is invalid ban updat Dependencies in execution Stage ");
        }
       var is_exsist= (from d in _dal.Dependence.ReadAll()
                        where d.IdPendingTask == dep && d.IdPreviousTask==prev
                        select d).FirstOrDefault(); 
        if (is_exsist != null)
        {

            throw new BlInvalidValueException("The dependency exists");
        }
        Dependence dependence = new Dependence() { IdPendingTask = dep, IdPreviousTask = prev };
        _dal.Dependence.Create(dependence);//Creates a dependency
    }

    public void Deletedep(int prev, int dep)//Deletes dependencies
    {
        DateTime? date = Schedule.getStartProjectDate();
        if (date != null)///Execution stage
        {
            throw new BlInvalidValueException("The Task data is invalid ban updat Dependencies in execution Stage ");
        }
        //Returns the id of the dependency
        var firstDep = (from d in _dal.Dependence.ReadAll()
                        where d.IdPendingTask == dep && d.IdPreviousTask == prev
                        select d.IdNum).FirstOrDefault();


        try 
        {
            if (firstDep != null)
            {
                _dal.Dependence.Delete(firstDep);//Trying to delete
            }
        }
        catch(DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"{ex}");
        }
        
    }
    /// <summary>
    /// List of recommended tasks for an engineer
    /// </summary>
    /// <param name="idE"></param>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>
    /// 
    public List<BO.Task> listTaskForEngineer(int idE)
    {
        try
        {
            BO.Engineer engineer = Engineer.Read(idE);

            var tasksWithoutEngineer = new List<BO.Task>();

            switch (engineer.Level)
            {
                case BO.EngineerLevel.Beginner:
                    tasksWithoutEngineer = (from t in Task.ReadAll()
                                            where t.Engineer == null && //No engineer is assigned to the task
                                               t.TaskLave == BO.EngineerLevel.Beginner && //at his level or lower than him
                                            t.Dependencies.All(dependency => dependency.Status == BO.Status.Done)//All his previous task are finish
                                            select t).ToList();
                    break;

                case BO.EngineerLevel.Advanced:
                    tasksWithoutEngineer = (from t in Task.ReadAll()
                                            where t.Engineer == null &&
                                            (t.TaskLave == BO.EngineerLevel.Advanced || t.TaskLave == BO.EngineerLevel.Beginner) &&
                                            t.Dependencies.All(dependency => dependency.Status == BO.Status.Done)
                                            select t).ToList();
                    break;

                case BO.EngineerLevel.AdvancedBeginner:
                    tasksWithoutEngineer = (from t in Task.ReadAll()
                                            where t.Engineer == null &&
                                                  (t.TaskLave == BO.EngineerLevel.AdvancedBeginner || t.TaskLave == BO.EngineerLevel.Advanced ||
                                                  t.TaskLave == BO.EngineerLevel.Beginner) &&
                                                  t.Dependencies.All(dependency => dependency.Status == BO.Status.Done)
                                            select t).ToList();
                    break;
                case BO.EngineerLevel.Intermediate:
                    tasksWithoutEngineer = (from t in Task.ReadAll()
                                            where t.Engineer == null &&
                                                  (t.TaskLave == BO.EngineerLevel.Intermediate || t.TaskLave == BO.EngineerLevel.AdvancedBeginner || t.TaskLave == BO.EngineerLevel.Advanced ||
                                                  t.TaskLave == BO.EngineerLevel.Beginner) &&
                                                  t.Dependencies.All(dependency => dependency.Status == BO.Status.Done)
                                            select t).ToList();
                    break;

                case BO.EngineerLevel.Expert:
                    //Can do all the levels because he is an expert
                    tasksWithoutEngineer = (from t in Task.ReadAll()
                                            where t.Engineer == null &&
                                                  t.Dependencies.All(dependency => dependency.Status == BO.Status.Done)
                                            select t).ToList();
                    break;
            }

            return tasksWithoutEngineer;
        }
        catch (BlDoesNotExistException ex) 
        {
            throw new BlDoesNotExistException($"Engineer with ID={idE} does Not exist");

        }

    }
  

}



