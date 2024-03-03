
namespace BlImplementation;
using BlApi;
using BO;
using System.Xml.Linq;
using Dal;
using DO;
using System;

internal class Bl : IBl
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public IEngineer Engineer =>  new EngineerImplementation();

   // public ITask Task => new TaskImplementation();
    public ITask Task => new TaskImplementation(this);//*** 


    public ISchedule Schedule => new ScheduleImplementation();

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




    public void depAdd (int prev, int dep)
    {
        if (Schedule.getStartProjectDate != null)
        {
            throw new BlInvalidValueException("The Task data is invalid ban updat Dependencies in execution Stage ");
        }
        Dependence dependence = new Dependence() { IdPendingTask = dep, IdPreviousTask = prev };
        _dal.Dependence.Create(dependence);
    }

    public void Deletedep(int prev, int dep)
    {
       if (Schedule.getStartProjectDate!= null)
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

}



