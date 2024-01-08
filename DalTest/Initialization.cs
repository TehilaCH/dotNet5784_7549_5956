
namespace DalTest;
using DalApi;
using DO;
using System;

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
            string fullName = engineerName;
            string name = engineerName.Replace(" ", "");//Reduces spaces in the string
            string email = $"{name}@gmail.com";//create email
            double cost = s_rand.Next(100, 500);//random cost
            EngineerLevel EngineerLevel = (EngineerLevel)s_rand.Next(0, 5);
            Engineer Engineeri = new Engineer(     //initialization feilds
            Name: fullName,
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
        string[] nameTask = { "New Software Development:", "Automated Tests: ", "Version Upgrade:", "Building an API:", "Bug Fixing:", "Development of new capabilities:" 
        ,"Optimization:","Information security:","Easy upgrade ways:","Shared code portfolio management:"
        ,"Integration of external tools:","Modular development:","Documentation:","Multi-platform code development:"
        ,"Performance fixes:","Establishing a cloud system:","Safety upgrade of existing versions:","Automatic build and distribution:"
        ,"Integrate advanced development tools:","Emulation and endurance test:"};

        string[] DescriptionTask = { "Develop a new system for managing tasks in your development team.", "Write scripts for automated tests for specific code.", "Upgrade the current version of the software to a new version and perform quality checks."
        ,"Open an API to support external services and external usability in your system.","Identify and fix known bugs in the source code.",
         "Add new capabilities to the software, such as support for additional languages or advanced capabilities."
        ,"Update and optimize the code to improve performance.","perform security checks and security upgrades."
        , " Provide easy upgrade and update ways for users.","Add a system for managing versions and code portfolios.",
        "integrate and use external development tools such as Git, Jenkins or Docker."," make the software modular to accommodate additional capabilities and modify existing modules."
        ,"create comprehensive documentation for the source code and various functions.","You will develop software that can run on different platforms such as Windows, macOS and Linux."
        ,"will improve performance and maximize the capabilities of the software."," move the software or part of it into a cloud environment."
        ,"updating old versions and viewing them in a safe manner.","setting up an automatic system for building and distributing new versions."
        ,"Improve development processes by using new and advanced tools.","test the software's ability to withstand high load and test its response in stress and extreme situations"};

        int i = 0;
        foreach (var t in nameTask)
        {

            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            DateTime creatTask = start.AddDays(s_rand.Next(range));//Task creation date
            string nickname = t;
            string description = DescriptionTask[i];
            Task taski = new Task()
            {
                Nickname = nickname,
                CreatTaskDate = creatTask,
                Description = description,
                Milestone = false,
                PlannedDateStartWork = null,
                StartDateTask = null,
                TimeRequired = null,
                Deadline = null,
                EndDate = null,
                Product = null,
                commentary = null,
                TaskLave = null

            };

            s_dalITask?.Create(taski);
            i++;

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







 
 

 
 

 



 

