namespace Dal;
using DalApi;
using DO;
using System.Data.SqlTypes;
using static Dal.DataSource;



internal class ScheduleImplementation : ISchedule
{
  
    public DateTime? getEndProjectDate()//A function that returns a date to a file
    {

        DateTime? date1 = Config.EndProjectDate;
        return date1;
    }

    public DateTime? getStartProjectDate()//A function that returns a date to a file
    {
       
        DateTime? date1 = Config.StartProjectDate;
        return date1;
    }

 

    public DateTime? SetEndProjectDate(DateTime date)//A function that writes a date to a file
    {
       
        DateTime? date1 = Config.EndProjectDate;
        if (date1 != null)//if There is a date
            return null;
        Config.StartProjectDate= date;
        return date;
        
    }

    public DateTime? SetStartProjectDate(DateTime date)//A function that writes a date to a file
    {
       
        DateTime? date1 = Config.StartProjectDate;
        if (date1 != null)//if There is a date
            return null;
        Config.StartProjectDate=date;
        return date;

    }
    public void resetTime()//Resets dates
    {
        Config.StartProjectDate = null;
        Config.EndProjectDate = null;
    }

    public void resetRunNumber()
    {
    }
}