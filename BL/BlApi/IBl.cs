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
        BlApi.Factory.Get.Schedule.resetTime();
        //איפס תארכים בקונפיגורציה 
    }



    DateTime Clock { get; }
    void AdvanceDay();
    void AdvanceHour();
    void AdvanceYear();
    void InitializeTime();


    public void depAdd(int prev, int dep);
    public void Deletedep(int prev, int dep);
    public List<BO.Task> listTaskForEngineer(int idE);
}
