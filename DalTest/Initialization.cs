
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
    private static readonly Random s_rand = new(); // A field for generating random numbers
    /// <summary>
    /// create Dependences random
    /// </summary>
    private static void creatDependences()
    {
        Random random = new Random();

        for (int i = 0; i < 40; i++)
        {
            int idPendingTask = i; 
            int idPreviousTask = random.Next(0, i);  

            Dependence dependence = new Dependence
            {
                IdPendingTask = idPendingTask,
                IdPreviousTask = idPreviousTask
            };

            s_dalIDependence?.Create(dependence);


        }
    }
    /// <summary>
    /// Creating 5 engineers and putting them on the list
    /// </summary>
    private static void creatEngineers()
    {
        
        string[] engineerNames =
            {
        "Moriya Atar", "Tehila Chen","Ortal Oren", "Lea Ashori",//list names
        "Moshe Lavi"
        };
        foreach (var engineerName in engineerNames)
        {
            int id;
            do
                id = s_rand.Next(200000000, 400000000);//id random
            while (s_dalIEngineer!.Read(id)!=null);
            string name = engineerName.Replace(" ", "");//Reduces spaces in the string
            string email = $"{name}@gmail.com";//create email
            double cost = s_rand.Next(100, 500);//random cost
            EngineerLevel EngineerLevel = (EngineerLevel)s_rand.Next(0, 5);
            Engineer Engineeri = new Engineer(     //initialization feilds
            IdNum: id,
            Email: email,
            CostPerHour: cost,
            EngineerLevel: EngineerLevel

            );


            s_dalIEngineer.Create(Engineeri);

        }

    }
    /// <summary>
    /// //creat 20 tasks
    /// </summary>
    private static void creatTasks() 
    {
        for (int i = 0; i < 20; i++)   //20 tasks
        {
            DateTime start = new DateTime(2020, 1, 1);
            int range = (DateTime.Today - start).Days;
            DateTime creatTask = start.AddDays(s_rand.Next(range));//Task creation date
             
            int range1 = (DateTime.Today - creatTask).Days;
            DateTime planingStart = creatTask.AddDays(s_rand.Next(range1));
             DateTime startWork = creatTask.AddDays(s_rand.Next(range1));
            int range2 = (DateTime.Today - startWork).Days;
            DateTime endTask = startWork.AddDays(s_rand.Next(range2));
            int range3 = (DateTime.Today - endTask).Days;
          
            string nickname = $"Task{i}";


            Task taski = new Task() { Nickname= nickname, CreatTaskDate= creatTask, PlannedDateStartWork= planingStart,
                StartDateTask= startWork,
                TimeRequired = null,
                Deadline = null,
                EndDate= endTask,
                Product = null,
                commentary = null,
                TaskLave=null
                
            };
       
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
