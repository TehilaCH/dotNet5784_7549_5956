
namespace BlImplementation;
using BlApi;
using BO;
using System.Xml.Linq;
using Dal;

internal class Bl : IBl
{
  
    public IEngineer Engineer =>  new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public ISchedule Schedule => new ScheduleImplementation();

    //public void InitializeDB() => DalTest.Initialization.Do();


}



