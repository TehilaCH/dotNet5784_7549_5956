
namespace DalTest;
using DalApi;
using DO;
/*using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;*/

public static class Initialization
{
    private static IDependence? s_dalIDependence;
    private static IEngineer? s_dalIEngineer;
    private static ITask? s_dalITask;
    private static readonly Random s_rand = new(); // שדה לייצירת מספרים רנדומלים

    private static void creatDependences()//********************
    {
        for(int i = 0; i < 40; i++)
        {
            Dependence dependencei = new Dependence();
                /*IdPendingTask:null,
                IdPendingTask: null
                );*/
            

            s_dalIDependence?.Create(dependencei);

        }
    }
    private static void creatEngineers()
    {
        
        string[] engineerNames =
            {
        "Moriya Atar", "Tehila Chen","Ortal Oren", "Lea Ashori",
        "Moshe Lavi"
        };
       // string[] Levels = { "Beginner", "Advanced", "AdvancedBeginner", "Intermediate", "Expert" };
        foreach (var engineerName in engineerNames)
        {
            int id;
            do
                id = s_rand.Next(200000000, 400000000);
            while (s_dalIEngineer!.Read(id)!=null);
            string name = engineerName.Replace(" ", "");//מורידה רווחים במחרוזת 
            string email = $"{name}@gmail.com";
            double cost = s_rand.Next(70, 200);
            Engineer Engineeri = new Engineer(
            IdNum: id,
            Email: email,
            CostPerHour: cost
            //EngineerLevel: עם אינם וסוויץ

            );


            s_dalIEngineer.Create(Engineeri);

        }

    }
    private static void creatTasks()
    {
        for (int i = 0; i < 20; i++)
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            DateTime creatTask = start.AddDays(s_rand.Next(range));//תאריך יצירת משימה
            string nickname = $"Task{i}";
            Task taski = new Task() { Nickname= nickname, CreatTaskDate= creatTask };
            //EngineerIdToTask-לבדוק אם צריך לאתחל את השדה הזה שמראה תץז של מהנדס למימה 
            s_dalITask?.Create(taski);


        }
      
    }
   
    public static void Do(IEngineer? dalEngineer, IDependence? dalDependence, ITask dalTask)
    {
        s_dalIEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dalITask= dalTask ?? throw new NullReferenceException("DAL can not be null!");
        s_dalIDependence= dalDependence ?? throw new NullReferenceException("DAL can not be null!");
        creatTasks();
        creatEngineers();
        creatDependences();

    }
}
