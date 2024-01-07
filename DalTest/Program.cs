using Dal;
using DalApi;
using DO;
using System;
using System.Data.Common;
using System.Xml.Linq;
namespace DalTest;

internal class Program
{
    private static IEngineer? s_dalIEngineer = new EngineerImplementation(); //stage 1
    private static IDependence? s_dalIDependence = new DependenceImplementation(); //stage 1
    private static ITask? s_dalITask = new TaskImplementation(); //stage 1

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
                        case "1":
                            Engineer engineer1 = initialEngineer();
                            int idE = s_dalIEngineer.Create(engineer1);
                            Console.WriteLine($"ID ={idE}");
                            break;
                        case "2":
                            Console.Write("Enter id: ");
                            int idR = int.Parse(Console.ReadLine());
                            Engineer? engineerRead = s_dalIEngineer.Read(idR);
                            Console.WriteLine(engineerRead);
                            break;
                        case "3":
                            Engineer engineer3 = initialEngineer();
                            s_dalIEngineer.Update(engineer3);
                            Console.WriteLine(engineer3);
                            break;
                        case "4":
                            Console.WriteLine("Enter id: ");
                            int idD = int.Parse(Console.ReadLine());
                            s_dalIEngineer.Delete(idD);
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
                        case "1":
                            Dependence dependence1 = initialDependenc();
                            int idD = s_dalIDependence.Create(dependence1);
                            Console.WriteLine($"ID ={idD}");
                            break;
                        case "2":
                            Console.WriteLine("Enter id: ");
                            int id2 = int.Parse(Console.ReadLine());
                            Dependence? dependence2 = s_dalIDependence.Read(id2);
                            Console.WriteLine(dependence2);
                            break;
                        case "3":
                            Console.WriteLine("Enter Dependence details:\n");
                            Console.WriteLine("Id: ");
                            int id = int.Parse(Console.ReadLine());
                            if (id == null)
                            {
                                break;
                            }
                            Console.WriteLine("\n");
                            Console.WriteLine("Id Pending Task: ");
                            int idPendingT = int.Parse(Console.ReadLine());
                            Console.WriteLine("\n");
                            Console.WriteLine("Id Previous Task: ");
                            int idPreviousT = int.Parse(Console.ReadLine());
                            Console.WriteLine("\n");
                            Dependence dependence3 = new Dependence() { IdNum = id, IdPendingTask = idPendingT, IdPreviousTask = idPreviousT };
                            s_dalIDependence.Update(dependence3);
                            break;
                        case "4":
                            Console.WriteLine("Id: ");
                            int id4 = int.Parse(Console.ReadLine());
                            s_dalIDependence.Delete(id4);
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
                        case "1":
                            DO.Task task1=initialTask();
                            int idT=s_dalITask.Create(task1);
                            Console.WriteLine($"ID={idT}");
                            break;
                        case"2":
                            Console.WriteLine("ID: ");
                            int id2=int.Parse(Console.ReadLine());
                           DO.Task? task2= s_dalITask.Read(id2);
                            Console.WriteLine(task2);
                            break;
                        case"3":
                            Console.WriteLine("ID: ");
                            int id3 = int.Parse(Console.ReadLine());//בקשר לת.ז צריך לטפל
                            DO.Task task3=initialTask();
                            s_dalITask.Update(task3);
                            break;
                        case "4":
                            Console.WriteLine("ID: ");
                            int id4 = int.Parse(Console.ReadLine());
                            s_dalITask.Delete(id4);
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


            /* try { }
             catch { }
             Console.WriteLine("Hello, World!");*/
            mainMenu();
            choice = int.Parse(Console.ReadLine());
        } while (choice!=0 );
    }
    static void mainMenu()
    {
        Console.WriteLine("choos one of the following:");
        Console.WriteLine("0:exit");
        Console.WriteLine("1:for Check Engineer");
        Console.WriteLine("2:for Check Dependence");
        Console.WriteLine("3:for Check Task");
    }
    static void menu()
    {
        Console.WriteLine("choos one of the following:");
        Console.WriteLine("0:exit");
        Console.WriteLine("1:Create");
        Console.WriteLine("2:Read");
        Console.WriteLine("3:Update");
        Console.WriteLine("4:delete");
    }
    static Engineer initialEngineer()
    {
        Console.WriteLine("Enter engineer details:");
        Console.Write("Id: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("Name: ");
        string name = Console.ReadLine();
        Console.Write("Email: ");
        string email = Console.ReadLine();
        //Console.WriteLine("EngineerLevel: ");//רמת מהנדס////לטפל באינם
        Console.Write("CostPerHour: ");
        double cost = double.Parse(Console.ReadLine());
        Engineer engineer = new Engineer() { IdNum = id, Name = name, Email = email, CostPerHour = cost };
        return engineer;
    }
    static Dependence initialDependenc()
    {
        Console.WriteLine("Enter Dependence details:\n");
        Console.WriteLine("Id Pending Task: ");
        int idPendingT = int.Parse(Console.ReadLine());
        Console.WriteLine("\n");
        Console.WriteLine("Id Previous Task: ");
        int idPreviousT = int.Parse(Console.ReadLine());
        Console.WriteLine("\n");
        Dependence dependence1 = new Dependence(){IdPendingTask = idPendingT, IdPreviousTask = idPreviousT};
        return dependence1;
    }
   static DO.Task initialTask()
    {
        Console.WriteLine("Nickname");
        string name = Console.ReadLine();
        Console.WriteLine("Description");
        string description = Console.ReadLine();
        Console.WriteLine("Milestone");
        bool Milestone1 = bool.Parse(Console.ReadLine());
        Console.WriteLine("Creat Task Date:");
        DateTime CreatTask = DateTime.Parse(Console.ReadLine());
        Console.WriteLine("Planned Date Start Work:");
        DateTime PlannedWork = DateTime.Parse(Console.ReadLine());
        Console.WriteLine("Start Date Task:");
        DateTime StartTask = DateTime.Parse(Console.ReadLine());
        Console.WriteLine("Time Required:");
        DateTime TimeRequired = DateTime.Parse(Console.ReadLine());
        Console.WriteLine("Deadline:");
        DateTime Deadline = DateTime.Parse(Console.ReadLine());
        Console.WriteLine("End Date:");
        DateTime EndDate = DateTime.Parse(Console.ReadLine());
        Console.WriteLine("Producte:");
        string Product = Console.ReadLine();
        Console.WriteLine("commentary:");
        string commentary = Console.ReadLine();
        Console.WriteLine("TaskLave:");
        string TaskLave1 = Console.ReadLine();
        DO.Task task1 = new DO.Task() {
            // EngineerIdToTask,
            Nickname = name,
            Description = description,
            Milestone = Milestone1,
            CreatTaskDate = CreatTask,
            PlannedDateStartWork = PlannedWork,
            StartDateTask = StartTask,
            TimeRequired = TimeRequired,
            Deadline = Deadline,
            EndDate = EndDate,
            Product = Product,
            commentary = commentary,
            TaskLave = null//לטפל באינם
        };
        return task1;
    }
   

}
