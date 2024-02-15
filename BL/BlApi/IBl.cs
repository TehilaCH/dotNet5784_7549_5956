using BO;

namespace BlApi;


public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }

    public ISchedule Schedule { get; }

    //public void InitializeDB();
    public static void InitializeDB() => DalTest.Initialization.Do();


    public static void ResetDB()
    {
        DalTest.Initialization.Reset();
        //איפס תארכים בקונפיגורציה 
    }
}
