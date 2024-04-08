using BO;

namespace BlApi;


public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }

    public ISchedule Schedule { get; }

    public static void InitializeDB() => DalTest.Initialization.Do();//Initializes database


    public static void ResetDB()//Data deletion
    {
        DalTest.Initialization.Reset();
        BlApi.Factory.Get.Schedule.resetTime();//Reset project dates(start and end)
        BlApi.Factory.Get.Schedule.resetRunNumber();//Reset running numbers to start at 1

    }



    DateTime Clock { get; }
    void AdvanceDay();//Increases days by 1
    void AdvanceHour();//Increases hours by 1
    void AdvanceYear();///Increases years by 1
    void InitializeTime();//current date


    public void depAdd(int prev, int dep);//Adds a dependency
    public void Deletedep(int prev, int dep);//delet a dependency
    public List<BO.Task> listTaskForEngineer(int idE);//Returns recommended tasks to the engineer
}
