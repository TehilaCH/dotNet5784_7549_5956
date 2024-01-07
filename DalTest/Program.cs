using Dal;
using DalApi;
using DO;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace DalTest;


internal class Program
{
    private static IEngineer? s_dalIEngineer = new EngineerImplementation(); //stage 1
    private static IDependence? s_dalIDependence = new DependenceImplementation(); //stage 1
    private static ITask? s_dalITask = new TaskImplementation(); //stage 1
    private static readonly Random s_rand= new();
    static void Main(string[] args)
    {
        mainMenu();
        int choice = int.Parse(Console.ReadLine());
        do
        {
            Initialization.Do(s_dalIEngineer, s_dalIDependence, s_dalITask);
            switch (choice)
            {
                case 1:
                    menu();
                    var choice1 = Console.ReadLine();
                    switch (choice1)
                    {
                        case "0":
                            break;
                            //create Engineer
                        case "1":
                            Console.WriteLine("Enter engineer details:");
                            Console.Write("Id: ");
                            int id1 = int.Parse(Console.ReadLine());
                            Engineer engineer1 = initialEngineer(id1);
                            try
                            {
                                int idE = s_dalIEngineer.Create(engineer1);
                                Console.WriteLine($"ID ={idE}");
                            }
                            catch (Exception ex)//Exception if there is such an id
                            {
                                Console.WriteLine($"{ex.Message}");
                            }
                            
                            break;
                        //read-return Engineer if found 
                        case "2":
                            Console.Write("Enter id: ");
                            int idR = int.Parse(Console.ReadLine());
                            Engineer? engineerRead = s_dalIEngineer.Read(idR);
                            if(engineerRead==null)
                                Console.WriteLine($"{idR} not found");
                            Console.WriteLine(engineerRead);
                            break;
                            //update Engineer 
                        case "3":
                            Console.Write("Enter Id: ");
                            int id3 = int.Parse(Console.ReadLine());
                            Engineer temp = s_dalIEngineer.Read(id3);
                            if (temp == null)
                            {
                                Console.WriteLine("not found this id to update");
                                break;
                            }
                            Console.WriteLine("Enter Details to be updated: ");
                            Engineer engineer3 = initialEngineer(id3);
                            try
                            {
                                s_dalIEngineer.Update(engineer3);
                                Console.WriteLine(engineer3);
                            }
                            catch (Exception ex)//Exception if not found
                            {
                                Console.WriteLine($"{ex.Message}");
                            }

                            break;
                        //Delete Engineer if exists
                        case "4":
                            Console.Write("Enter id: ");
                            int idD = int.Parse(Console.ReadLine());
                            try 
                            { 
                                s_dalIEngineer.Delete(idD);
                            }
                            catch (Exception ex)//Exception if not exists
                            {
                                Console.WriteLine($"{ex.Message}");
                            }
                            break;
                        case "5":
                            List<Engineer> CopyEngineers = new List<Engineer>();
                            CopyEngineers =s_dalIEngineer.ReadAll();
                           foreach(var engineer in CopyEngineers)
                           {
                                Console.WriteLine(engineer);

                           }
                            break;

                        default:
                            Console.WriteLine("Invalid selection");
                            break;
                    }
                    break;
               
        
                case 2:
                    menu();
                    var choice2 = Console.ReadLine();
                    switch (choice2)
                    {
                        case "0":
                            break;
                        //create Dependence
                        case "1":
                            Dependence dependence1 = initialDependenc();
                            try 
                            {
                                int idD = s_dalIDependence.Create(dependence1);
                                Console.WriteLine($"ID ={idD}");
                            }
                            catch (Exception ex)//Exception if exists
                            {
                                Console.WriteLine($"{ex.Message}");
                            }
                            break;
                        //read-search Dependence and return if exsist else return null
                        case "2":
                            Console.Write("Enter id: ");
                            int id2 = int.Parse(Console.ReadLine());
                            Dependence? dependence2 = s_dalIDependence.Read(id2);
                            if(dependence2==null)
                                Console.WriteLine($"{id2} not found");
                            Console.WriteLine(dependence2);
                            break;
                            //updates a dependency if exsist
                        case "3":
                            Console.WriteLine("Enter Dependence details:");
                            Console.Write("Id: ");
                            int id= int.Parse(Console.ReadLine());
                            Dependence temp =s_dalIDependence.Read(id);
                            if (temp == null)//will not update if entered 0
                            {
                                Console.WriteLine("not found this id to update");
                                break;
                            }
                            Console.Write("Id Pending Task: ");
                            int IdPendingTask = int.Parse(Console.ReadLine());
                            Console.Write("Id Previous Task: ");
                            int idPreviousT = int.Parse(Console.ReadLine());
                            Dependence dependence3 = new Dependence() { IdNum = id, IdPendingTask = IdPendingTask, IdPreviousTask = idPreviousT };
                            try
                            { 
                                s_dalIDependence.Update(dependence3);
                                Console.WriteLine(dependence3);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"{ex.Message}");//Exception if not exsist
                            }
                            break;
                        //Delete Dependence if exsist
                        case "4":
                            Console.Write("Id: ");
                            int id4 = int.Parse(Console.ReadLine());
                            try
                            { s_dalIDependence.Delete(id4); }
                            catch (Exception ex)//Exception if not exsist
                            {
                                Console.WriteLine($"{ex.Message}");
                            }
                            break;
                        case "5":
                            List<Dependence> CopyDependences = new List<Dependence>();
                            CopyDependences = s_dalIDependence.ReadAll();
                            foreach (var dependence in CopyDependences)
                            {
                                Console.WriteLine(dependence);

                            }
                            break;
                        default:
                            Console.WriteLine("Invalid selection");
                            break;
                    }
                    break;

                case 3:
                    menu();
                    var choice3 = Console.ReadLine();
                    switch(choice3)
                    {
                        case "0":
                            break;
                        //Create Task
                        case "1":
                            DO.Task task1=initialTask();
                            try
                            {
                                int idT = s_dalITask.Create(task1);
                                Console.WriteLine($"ID={idT}");
                            }
                            catch (Exception ex)//Exception if already exists
                            {
                                Console.WriteLine($"{ex.Message}");
                            }
                            break;
                            //read-search Task and return if exsist else return null
                        case"2":
                            Console.Write("Enter id: ");
                            int id=int.Parse(Console.ReadLine());
                            DO.Task? task2= s_dalITask.Read(id);
                            //if (task2 == null)
                              //  Console.WriteLine($"{id2} not found");
                            Console.WriteLine(task2);
                            break;
                        //Update Task exsist
                        case "3":
                            DO.Task task3= updatTask() ;
                            try 
                            {
                                s_dalITask.Update(task3);
                                Console.WriteLine(task3);
                            }   
                            catch (Exception ex)//Exception if task not exsist
                            {
                                Console.WriteLine($"{ex.Message}");
                            }
                            break;
                        //Delete task if found
                        case "4":
                            Console.Write("ID: ");
                            int id4 = int.Parse(Console.ReadLine());
                            try { s_dalITask.Delete(id4); }
                            catch (Exception ex)//Exception if task not found
                            {
                                Console.WriteLine($"{ex.Message}");
                            }
                            break;
                        case "5":
                            List<DO.Task> copyTasks = new List<DO.Task>();
                            copyTasks = s_dalITask.ReadAll();
                            foreach (var task in copyTasks)
                            {
                                Console.WriteLine(task);

                            }
                            break;
                        default:
                            Console.WriteLine("Invalid selection");
                            break;
                    }
                    break;
                
                default:
                    Console.WriteLine("Invalid selection");
                    break;
            }
            mainMenu();
            choice = int.Parse(Console.ReadLine());
        } while (choice!=0 );
    }



    //help functions 
    static void mainMenu()//main menu
    {
        Console.WriteLine("choos one of the following:");
        Console.WriteLine("0:exit");
        Console.WriteLine("1:for Check Engineer");
        Console.WriteLine("2:for Check Dependence");
        Console.WriteLine("3:for Check Task");
    }
    static void menu()//sub menu
    {
        Console.WriteLine("choos one of the following:");
        Console.WriteLine("0:exit");
        Console.WriteLine("1:Create");
        Console.WriteLine("2:Read");
        Console.WriteLine("3:Update");
        Console.WriteLine("4:delete");
        Console.WriteLine("5:readAll");

    }
    /// <summary>
    ///  create and initialization Engineer
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    static Engineer initialEngineer(int id)
    {
        Console.Write("Name: ");
        string name = Console.ReadLine();
        Console.Write("Email: ");
        string email = Console.ReadLine();
        Console.Write("Cost Per Hour: ");
        double cost = double.Parse(Console.ReadLine());
        EngineerLevel level = (EngineerLevel) s_rand.Next(0,5);
        Engineer engineer = new Engineer() { IdNum = id, Name = name, Email = email, CostPerHour = cost, EngineerLevel= level };
        return engineer;
    }
    /// <summary>
    /// create and initialization Dependence
    /// </summary>
    /// <returns></returns>
    static Dependence initialDependenc()
    {
        Console.WriteLine("Enter Dependence details:");
        Console.Write("Id Pending Task: ");
        int idPendingT = int.Parse(Console.ReadLine());
        Console.Write("Id Previous Task: ");
        int idPreviousT = int.Parse(Console.ReadLine());
        Dependence dependence1 = new Dependence(){IdPendingTask = idPendingT, IdPreviousTask = idPreviousT};
        return dependence1;
    }
    /// <summary>
    /// create and initialization Task
    /// </summary>
    /// <returns></returns>
    static DO.Task initialTask()
    {
        Console.Write("Nickname:");
        string name = Console.ReadLine();
        Console.Write("Description:");
        string description = Console.ReadLine();
       // Console.Write("Milestone:");
        //bool Milestone1 = bool.Parse(Console.ReadLine());
        Console.Write("Creat Task Date:");
        DateTime CreatTask = DateTime.Parse(Console.ReadLine());
        Console.Write("Planned Date Start Work:");
        DateTime PlannedWork = DateTime.Parse(Console.ReadLine());
        Console.Write("Start Date Task:");
        DateTime StartTask = DateTime.Parse(Console.ReadLine());
        Console.Write("Time Required:");
        DateTime TimeRequired = DateTime.Parse(Console.ReadLine());
        Console.Write("Deadline:");
        DateTime Deadline = DateTime.Parse(Console.ReadLine());
        Console.Write("End Date:");
        DateTime EndDate = DateTime.Parse(Console.ReadLine());
        Console.Write("Producte:");
        string Product = Console.ReadLine();
        Console.Write("commentary:");
        string commentary = Console.ReadLine();
        Console.Write("Engineer Id To Task:");
        int engineerId = int.Parse(Console.ReadLine());
        EngineerLevel TaskLave = (EngineerLevel)s_rand.Next(0, 5);
        DO.Task task1 = new DO.Task() {
            EngineerIdToTask = engineerId,
            Nickname = name,
            Description = description,
            Milestone = false,
            CreatTaskDate = CreatTask,
            PlannedDateStartWork = PlannedWork,
            StartDateTask = StartTask,
            TimeRequired = TimeRequired,
            Deadline = Deadline,
            EndDate = EndDate,
            Product = Product,
            commentary = commentary,
            TaskLave = TaskLave
        };
        return task1;
    }
   
    static DO.Task updatTask ()//updat Task
    {
        Console.Write("ID to updat: ");
        int id3 = int.Parse(Console.ReadLine());
        Console.Write("Nickname:");
        string name = Console.ReadLine();
        Console.Write("Description:");
        string description = Console.ReadLine();
        Console.Write("Milestone:");
        bool Milestone1 = bool.Parse(Console.ReadLine());
        Console.Write("Creat Task Date:");
        DateTime CreatTask = DateTime.Parse(Console.ReadLine());
        Console.Write("Planned Date Start Work:");
        DateTime PlannedWork = DateTime.Parse(Console.ReadLine());
        Console.Write("Start Date Task:");
        DateTime StartTask = DateTime.Parse(Console.ReadLine());
        Console.Write("Time Required:");
        DateTime TimeRequired = DateTime.Parse(Console.ReadLine());
        Console.Write("Deadline:");
        DateTime Deadline = DateTime.Parse(Console.ReadLine());
        Console.Write("End Date:");
        DateTime EndDate = DateTime.Parse(Console.ReadLine());
        Console.Write("Producte:");
        string Product = Console.ReadLine();
        Console.Write("commentary:");
        string commentary = Console.ReadLine();
        Console.Write("Engineer Id To Task:");
        int EngineerId =int.Parse( Console.ReadLine());
        EngineerLevel TaskLave = (EngineerLevel)s_rand.Next(0, 5);
        DO.Task task1 = new DO.Task() { TaskId = id3, EngineerIdToTask= EngineerId, Nickname= name,
            Description= description,Milestone= Milestone1,CreatTaskDate= CreatTask,
            PlannedDateStartWork= PlannedWork,
            StartDateTask= StartTask,
            TimeRequired= TimeRequired, Deadline= Deadline,
            EndDate= EndDate,
            Product= Product,
            commentary = commentary,
            TaskLave= TaskLave
        };
      
        return task1;
    }
}
