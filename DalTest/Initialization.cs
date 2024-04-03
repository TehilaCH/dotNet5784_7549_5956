
namespace DalTest;
using DalApi;
using DO;
using System;
public static class Initialization
{
    
    private static IDal? s_dal;


    private static readonly Random s_rand = new(); // A field for generating random numbers
    /// <summary>
    /// create Dependences random
    /// </summary>
    private static void creatDependences()
    {
        Random random = new Random();

        for (int i = 0; i < 40; i++)//Initialization of 40 dependencies
        {
            int idPendingTask = i; 
            int idPreviousTask = random.Next(0, i);//The task depends on its Previous task

            Dependence dependence = new Dependence//creating a entity
            {
                IdPendingTask = idPendingTask,
                IdPreviousTask = idPreviousTask
            };

            s_dal?.Dependence.Create(dependence);//creating a dependency


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
            while (s_dal!.Engineer.Read(id)!=null);//If such an id exists, create a new id
            string fullName = engineerName;
            string name = engineerName.Replace(" ", "");//Reduces spaces in the string
            string email = $"{name}@gmail.com";//create email
            double cost = s_rand.Next(100, 500);//random cost
            EngineerLevel EngineerLevel = (EngineerLevel)s_rand.Next(0, 5);
            Engineer Engineeri = new Engineer(     //creat and initialization feilds
            Name: fullName,
            IdNum: id,
            Email: email,
            CostPerHour: cost,
            EngineerLevel: EngineerLevel

            );


            s_dal.Engineer.Create(Engineeri);//creates an engineer

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

            DateTime start = new DateTime(1995, 1, 1);//start date
            int range = (DateTime.Today - start).Days;
            DateTime creatTask = start.AddDays(s_rand.Next(range));//Task creation date
            string nickname = t;
            string description = DescriptionTask[i];//Description from the array of descriptions
            EngineerLevel level = (EngineerLevel)s_rand.Next(0, 5);
            Task taski = new Task()//Create a task
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
                Remarks = null,
                TaskLave = level

            };

            s_dal?.Task.Create(taski);//Sending to a create function
            i++;

        }

    }
   
    public static void Do()//Initializing all data
    {
         s_dal = Factory.Get; 
        creatTasks();
        creatEngineers();
        creatDependences();

    }

    public static void Reset()//Deletes all data
    {
        //Do();
        /**/
        s_dal = Factory.Get;
        s_dal.Engineer.clear();
        s_dal.Dependence.clear();
        s_dal.Task.clear();

    }
}







 
 

 
 

 



 

