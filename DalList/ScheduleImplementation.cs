namespace Dal;
using DalApi;
using DO;
using System.Data.SqlTypes;
using static Dal.DataSource;



internal class ScheduleImplementation : ISchedule
{
    private readonly string _dataConfigXml = "data-config";
    public DateTime? getEndProjectDate()//A function that returns a date to a file
    {
   
        DateTime? date1 = Config.StartProjectDate;
        return date1;
    }

    public DateTime? getStartProjectDate()//A function that returns a date to a file
    {
       
        DateTime? date1 = Config.StartProjectDate;
        return date1;
    }

 

    public DateTime? SetEndProjectDate(DateTime date)//A function that writes a date to a file
    {
       
        DateTime? date1 = Config.StartProjectDate;
        if (date1 != null)
            return null;
        Config.StartProjectDate= date;
        return date;
        
    }

    public DateTime? SetStartProjectDate(DateTime date)//A function that writes a date to a file
    {
       
        DateTime? date1 = Config.StartProjectDate;
        if (date1 != null)
            return null;
        Config.StartProjectDate=date;
        return date;

    }
    public void resetTime()
    {
        throw new NotImplementedException();
    }
}