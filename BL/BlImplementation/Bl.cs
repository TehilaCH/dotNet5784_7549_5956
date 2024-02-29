
namespace BlImplementation;
using BlApi;
using BO;
using System.Xml.Linq;
using Dal;

internal class Bl : IBl
{
  
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
}



