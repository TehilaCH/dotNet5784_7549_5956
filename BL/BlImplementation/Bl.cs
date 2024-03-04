
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

   // public ITask Task => new TaskImplementation();
    public BlApi.ITask Task => new TaskImplementation(this);//*** 


    public BlApi.ISchedule Schedule => new ScheduleImplementation();

    //public void InitializeDB() => DalTest.Initialization.Do();


    private static DateTime s_Clock = DateTime.Now.Date;
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }

    public void AdvanceDay()
    {
        s_Clock = s_Clock.AddDays(1);
    }

    public void AdvanceHour()
    {
        s_Clock = s_Clock.AddHours(1);
    }

    public void AdvanceYear()
    {
        s_Clock = s_Clock.AddYears(1);
    }
    public void InitializeTime()
    {
        s_Clock = DateTime.Now;
    }


    public void depAdd(int prev, int dep)
    {
        DateTime? date = Schedule.getStartProjectDate();
        if (date != null)
        {
            throw new BlInvalidValueException("The Task data is invalid ban updat Dependencies in execution Stage ");
        }
        Dependence dependence = new Dependence() { IdPendingTask = dep, IdPreviousTask = prev };
        _dal.Dependence.Create(dependence);
    }

    public void Deletedep(int prev, int dep)
    {
        DateTime? date = Schedule.getStartProjectDate();
        if (date != null)
        {
            throw new BlInvalidValueException("The Task data is invalid ban updat Dependencies in execution Stage ");
        }
        var firstDep = (from d in _dal.Dependence.ReadAll()
                        where d.IdPendingTask == dep && d.IdPreviousTask == prev
                        select d.IdNum).FirstOrDefault();



        if (firstDep != null)
        {
            _dal.Dependence.Delete(firstDep);
        }
    }

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
                                            where t.Engineer == null &&
                                            t.TaskLave == BO.EngineerLevel.Beginner &&
                                            t.Dependencies.All(dependency => dependency.Status == BO.Status.Done)
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
                    // כאן המהנדס מתאים לכל משימות
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



