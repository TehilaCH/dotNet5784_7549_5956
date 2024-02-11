namespace BlTest;
using BO;



internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get;
    private static readonly Random s_rand = new();

    static void Main(string[] args)
    {
        Console.Write("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
            DalTest.Initialization.Do();
        mainMenu();
        int choice = int.Parse(Console.ReadLine()!);
        do
        {
            switch (choice)
            {
                case 1:
                    menuEngineer();
                    var choice1 = Console.ReadLine();
                    switch (choice1)
                    {
                        case "0":
                            break;
                        //create Engineer
                        case "1":
                            BO.Engineer engineer = Engineerinitialization();
                            try
                            {
                                int idE = s_bl.Engineer.Creat(engineer);
                                Console.WriteLine($"ID ={idE}");
                            }
                            catch (Exception ex)//Exception if there is such an id
                            {
                                Console.WriteLine($"{ex.Message}");
                            }
                            break;
                        //read-return Engineer if found 
                        case "2":
                            Console.Write("Enter Id for Engineer:");
                            int Id = int.Parse(Console.ReadLine()!);
                            try
                            {
                                BO.Engineer? engineerRead = s_bl.Engineer.Read(Id);
                                // engineerRead.ToString();
                                Console.WriteLine(engineerRead.ToString());
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"{ex.Message}");

                            }
                            break;
                        //update Engineer 
                        case "3":
                            BO.Engineer engineerU = Engineerinitialization();
                            try
                            {
                                s_bl.Engineer.Update(engineerU);
                                Console.WriteLine(engineerU.ToString());
                            }
                            catch (Exception ex)//Exception if not found
                            {
                                Console.WriteLine($"{ex.Message}");
                            }

                            break;
                        //Delete Engineer if exists
                        case "4":
                            Console.Write("Enter id: ");
                            int idD = int.Parse(Console.ReadLine()!);
                            try
                            {
                                s_bl.Engineer.Delete(idD);
                            }
                            catch (Exception ex)//Exception if not exists
                            {
                                Console.WriteLine($"{ex.Message}");
                            }
                            break;
                        //ReadAll Engineers
                        case "5":
                            IEnumerable<BO.Engineer> CopyEngineers = new List<BO.Engineer>();
                            CopyEngineers = s_bl.Engineer.ReadAll();
                            foreach (var engineerR in CopyEngineers)
                            {
                                Console.WriteLine(engineerR.ToString());

                            }
                            break;
                        case "6":

                            IEnumerable<BO.Engineer> Engineers= s_bl.Engineer.OrderEngineers();
                            foreach (var engineerR in Engineers)
                            {
                                Console.WriteLine(engineerR.ToString());

                            }
                            break;
                        case "7":
                            var engineersGrouped = s_bl.Engineer.GroupByEngineerLevel();
                            foreach (var engineerLevel in engineersGrouped)
                            {
                                Console.WriteLine($"Engineers at level: {engineerLevel.Key}");
                                foreach (var engineerItem in engineerLevel.Value) 
                                {
                                    Console.WriteLine($"ID: {engineerItem.Id}, Name: {engineerItem.Name}, Email: {engineerItem.Email}, Cost: {engineerItem.Cost}, Level: {engineerItem.Level}");
                                    if (engineerItem.Task != null)
                                    {
                                        Console.WriteLine($"Task ID: {engineerItem.Task.Id}, Nickname: {engineerItem.Task.NickName}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("No task assigned.");
                                    }
                                }
                                Console.WriteLine();
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid selection");
                            break;
                    }
                    break;


                case 2:
                    menuTask();
                    var choice2 = Console.ReadLine();
                    switch (choice2)
                    {
                        case "0":
                            break;
                        //Create Task
                        case "1":
                            BO.Task task = TaskInitialization();
                            try
                            {
                                int idT = s_bl.Task.Creat(task);
                                Console.WriteLine($"ID={idT}");
                            }
                            catch (BlInvalidValueException ex)//Exception if already exists
                            {
                                Console.WriteLine($"{ex.Message}");

                                //Exception
                            }
                            break;
                        //read-search Task and return if exsist else return null
                        case "2":
                            Console.Write("Enter id: ");
                            int id = int.Parse(Console.ReadLine()!);
                            try
                            {
                                BO.Task? task2 = s_bl.Task.Read(id);
                                Console.WriteLine(task2.ToString());
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"{ex.Message}");

                            }
                            break;
                        //Update Task exsist
                        case "3":
                            BO.Task task1 = TaskUpdate();
                            try
                            {
                                s_bl.Task.Update(task1);
                                Console.WriteLine(task1.ToString());
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"{ex.Message}");

                            }

                            break;
                        //Delete task if found
                        case "4":
                            Console.Write("Enter id to delete: ");
                            int idD = int.Parse(Console.ReadLine()!);
                            try
                            {
                                s_bl.Task.Delete(idD);
                            }

                            catch (Exception ex)
                            {
                                Console.WriteLine($"{ex.Message}");
                            }

                            break;
                        //ReadAll task
                        case "5":
                            IEnumerable<BO.Task> copyTasks = new List<BO.Task>();
                            copyTasks = s_bl.Task.ReadAll();
                            foreach (var t in copyTasks)
                            {
                                Console.WriteLine(t.ToString());

                            }

                            break;
                        //UpdateStartDate
                        case "6":
                            Console.Write("Enter id: ");
                            int idU = int.Parse(Console.ReadLine()!);
                            Console.Write("Enter date: ");
                            DateTime date = DateTime.Parse(Console.ReadLine()!);
                            try
                            {
                                s_bl.Task.UpdateStartDate(idU, date);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"{ex.Message}");
                            }
                            break;
                        
                        default:
                            Console.WriteLine("Invalid selection");
                            break;
                    }
                    break;
                case 3:
                    s_bl.Engineer.clear();
                    s_bl.Task.clear();

                    Console.Write("Would you like to create Initial data? (Y/N)");
                    string? ans2 = Console.ReadLine() ?? throw new FormatException("Wrong input");
                    if (ans2 == "Y")
                    {
                        
                        DalTest.Initialization.Do();
                    }
                    break;
                case 4:
                    Console.WriteLine("Enter date: ");
                    string userInput = Console.ReadLine();

                    DateTime? date1 = ReadNullableDateTime(userInput);

                    try
                    {
                        if (date1 == null)
                        {
                            Console.WriteLine("Data is not correct");
                        }
                        else
                        {
                            DateTime date2 = date1.Value; 
                            BlApi.Factory.Get.SetStartProjectDate(date2);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                    break;
                default:
                    Console.WriteLine("Wrong input");
                    break;
            }
            mainMenu();
            choice = int.Parse(Console.ReadLine()!);
        } while (choice != 0);

    }
    static void mainMenu()//main menu
    {
        Console.WriteLine("choos one of the following:");
        Console.WriteLine("0:exit");
        Console.WriteLine("1:for Check Engineer");
        Console.WriteLine("2:for Check Task");
        Console.WriteLine("3:for reset/clear data");
        Console.WriteLine("4:for update Start Project Date: ");
    }
    static void menuEngineer()//sub menu
    {
        Console.WriteLine("choos one of the following:");
        Console.WriteLine("0:exit");
        Console.WriteLine("1:Create");
        Console.WriteLine("2:Read");
        Console.WriteLine("3:Update");
        Console.WriteLine("4:delete");
        Console.WriteLine("5:readAll");
        Console.WriteLine("6:Order Engineers");
        Console.WriteLine("7:GroupBy EngineerLevel");
    }
    static void menuTask()//sub menu
    {
        Console.WriteLine("choos one of the following:");
        Console.WriteLine("0:exit");
        Console.WriteLine("1:Create");
        Console.WriteLine("2:Read");
        Console.WriteLine("3:Update");
        Console.WriteLine("4:delete");
        Console.WriteLine("5:readAll");
        Console.WriteLine("6:UpdateStartDate");
       
    }

    static BO.Engineer Engineerinitialization()
    {
        Console.WriteLine("Enter engineer details ");
        Console.Write("Enter Id:");
        int id = int.Parse(Console.ReadLine()!);
        Console.Write("Enter Name:");
        string? name = ReadNullableString(Console.ReadLine()!);
        Console.Write("Enter Email:");
        string? email = ReadNullableString(Console.ReadLine()!);
        Console.Write("Enter cost per hour :");
        double? cost = ReadNullableDouble(Console.ReadLine()!);

        Console.WriteLine("Enter engineer level (Beginner, Advanced, AdvancedBeginner, Intermediate, Expert):");
        BO.EngineerLevel? EnginerLave;
        string input2 = Console.ReadLine();

        if (Enum.TryParse(input2, out BO.EngineerLevel result2))
        {
            EnginerLave = (BO.EngineerLevel?)result2;

        }
        else EnginerLave = null;
        //BO.EngineerLevel EnginerLave = (BO.EngineerLevel)s_rand.Next(0, 5);
        Console.Write("Enter idTask for engineer :");
        int? IdTask1 = ReadNullableInt(Console.ReadLine()!);
        Console.Write("Enter NickName for engineer :");
        string? NickName1 = ReadNullableString(Console.ReadLine()!);
        
        TaskInEngineer? task = null;
        if (IdTask1 != null || NickName1 != null)
        {
            task = new TaskInEngineer { Id = IdTask1, NickName = NickName1 };
        }
        BO.Engineer engineer = new BO.Engineer { Id = id, Name = name, Email = email, Cost = cost, Level = EnginerLave, Task = task };
        return engineer;
    }

    static double? ReadNullableDouble(string input)
    {
        if (string.IsNullOrEmpty(input))
            return null;
        else
            return double.Parse(input);
    }

    static BO.Task TaskUpdate()
    {
        Console.Write("Enter Id Task to Update: ");
        int id1 = int.Parse(Console.ReadLine()!);
        Console.Write("Nickname:");
        string? name1 = ReadNullableString(Console.ReadLine()!);
        Console.Write("Description:");
        string? description1 = ReadNullableString(Console.ReadLine()!);
        BO.Task T=s_bl.Task.Read(id1);
        DateTime? CreatTask1 = T.CreatTaskDate;   //DateTime.Now;
        Console.Write("Planned Date Start Work:");
        DateTime? PlannedWork1 = ReadNullableDateTime(Console.ReadLine()!);
        Console.Write("Start Date Task:");
        DateTime? StartTask1 = ReadNullableDateTime(Console.ReadLine()!);
        Console.Write("Time Required:");
        TimeSpan? TimeRequired1 = ReadNullableTimeSpan(Console.ReadLine()!);
        Console.Write("Deadline:");
        DateTime? Deadline1 = ReadNullableDateTime(Console.ReadLine()!);
        Console.Write("End Date:");
        DateTime? EndDate1 = ReadNullableDateTime(Console.ReadLine()!);
        Console.Write("Producte:");
        string? Product1 = ReadNullableString(Console.ReadLine()!);
        Console.Write("Remarks:");
        string? Remarks1 = ReadNullableString(Console.ReadLine()!);
        Console.Write("Engineer Id To Task:");
        int? Id = ReadNullableInt(Console.ReadLine()!);
        Console.Write("Enter name for engineer :");
        string? name = ReadNullableString(Console.ReadLine()!);

        EngineerInTask? engineer = null;
        if (Id != null || name != null)
        {
            engineer = new EngineerInTask { Id = Id, Name = name };
        }
        Console.WriteLine("Enter status (Unscheduled, Scheduled, OnTrack, Done): ");
        Status? stat1;
        string input = Console.ReadLine();

        if (Enum.TryParse(input, out Status result))
        {
            stat1 = result;

        }
        else stat1 = null;



        Console.WriteLine("Enter engineer level (Beginner, Advanced, AdvancedBeginner, Intermediate, Expert):");
        BO.EngineerLevel? TaskLave1;
        string input2 = Console.ReadLine();

        if (Enum.TryParse(input2, out BO.EngineerLevel result2))
        {
            TaskLave1 = (BO.EngineerLevel?)result2;

        }
        else TaskLave1 = null;
        List<TaskInList>? dep1 = Dependencies();

        BO.Task task1 = new BO.Task()
        {
            Id = id1,
            NickName = name1,
            Description = description1,
            CreatTaskDate = CreatTask1,
            PlannedDateStartWork = PlannedWork1,
            StartDateTask = StartTask1,
            TimeRequired = TimeRequired1,
            Deadline = Deadline1,
            EndDate = EndDate1,
            Product = Product1,
            Remarks = Remarks1,
            TaskLave = TaskLave1,
            Status = stat1,
            Engineer = engineer,
            Dependencies = dep1

        };
        return task1;
    }

    static BO.Task TaskInitialization()
    {
        Console.WriteLine("Enter Task details: ");
        Console.Write("Nickname:");
        string? name = ReadNullableString(Console.ReadLine()!);
        Console.Write("Description:");
        string? description = ReadNullableString(Console.ReadLine()!);
        DateTime? CreatTask = DateTime.Now;
        Console.Write("Planned Date Start Work:");
        DateTime? PlannedWork = ReadNullableDateTime(Console.ReadLine()!);
        Console.Write("Start Date Task:");
        DateTime? StartTask = ReadNullableDateTime(Console.ReadLine()!);
        Console.Write("Time Required:");
        TimeSpan? TimeRequired = ReadNullableTimeSpan(Console.ReadLine()!);
        Console.Write("Deadline:");
        DateTime? Deadline = ReadNullableDateTime(Console.ReadLine()!);
        Console.Write("End Date:");
        DateTime? EndDate = ReadNullableDateTime(Console.ReadLine()!);
        Console.Write("Product:");
        string? Product = ReadNullableString(Console.ReadLine()!);
        Console.Write("Remarks:");
        string? Remarks = ReadNullableString(Console.ReadLine()!);
        Console.Write("Engineer Id To Task:");
        int? IdE = ReadNullableInt(Console.ReadLine()!);
        Console.Write("Enter name for engineer :");
        string? Ename = ReadNullableString(Console.ReadLine()!);

        EngineerInTask? engineer = null;
        if (IdE != null || Ename != null)
        {
            engineer = new EngineerInTask { Id = IdE, Name = Ename };
        }

        Console.WriteLine("Enter status (Unscheduled, Scheduled, OnTrack, Done): ");
        Status? stat;
        string input = Console.ReadLine();

        if (Enum.TryParse(input, out Status result))
        {
            stat = result;

        }
        else stat = null;


       
        Console.WriteLine("Enter Task level (Beginner, Advanced, AdvancedBeginner, Intermediate, Expert):");
        BO.EngineerLevel? TaskLave;
        string input2 = Console.ReadLine();

        if (Enum.TryParse(input2, out BO.EngineerLevel result2))
        {
            TaskLave = (BO.EngineerLevel?)result2;

        }
        else TaskLave = null;

        List<TaskInList>? dep = Dependencies();
       // BO.EngineerLevel TaskLave = (BO.EngineerLevel)s_rand.Next(0, 5);
        //Status stat = (Status)s_rand.Next(0, 4);

        BO.Task task = new BO.Task()
        {
            NickName = name,
            Description = description,
            CreatTaskDate = CreatTask,
            PlannedDateStartWork = PlannedWork,
            StartDateTask = StartTask,
            TimeRequired = TimeRequired,
            Deadline = Deadline,
            EndDate = EndDate,
            Product = Product,
            Remarks = Remarks,
            TaskLave = TaskLave,
            Status = stat,
            Engineer =engineer,
            Dependencies= dep
        };
        return task;
    }



    static DateTime? ReadNullableDateTime(string input)
    {
        if (string.IsNullOrEmpty(input))
            return null;
        return DateTime.Parse(input);
    }

    static TimeSpan? ReadNullableTimeSpan(string input)
    {
        if (string.IsNullOrEmpty(input))
            return null;
        return TimeSpan.Parse(input);
    }

    static int? ReadNullableInt(string input)
    {
        if (string.IsNullOrEmpty(input))
            return null;
        return int.Parse(input);
    }
    static string? ReadNullableString(string input)
    {
        if (string.IsNullOrEmpty(input))
            return null;
        else
            return input;
    }




    public static List<TaskInList>? Dependencies()
    {
        List<TaskInList> tasks = new List<TaskInList>();

        Console.WriteLine("Enter tasks dep:");
        while (true)
        {
            Console.Write("Enter task ID: ");
            string idInput = Console.ReadLine();

            // If input is empty, break out of the loop
            if (string.IsNullOrWhiteSpace(idInput))
            {
                break;
            }

            int id;
            if (!int.TryParse(idInput, out id))
            {
                Console.WriteLine("Invalid input ");
               // continue;
            }

            Console.Write("Enter task description: ");
            string description = Console.ReadLine();

            Console.Write("Enter task nickname: ");
            string nickName = Console.ReadLine();

            Console.WriteLine("Enter status (Unscheduled, Scheduled, OnTrack, Done): ");
            string statusInput = Console.ReadLine();

            Status status;
            if (!Enum.TryParse(statusInput, out status))
            {
                //Console.WriteLine("Invalid input for status. Please enter a valid status.");
                //continue;
            }

            TaskInList task = new TaskInList { Id = id, NickName = nickName, Description = description, Status = status };
            tasks.Add(task);
        }
        // If no tasks were added, return null
        if (tasks.Count == 0)
        {
            return null;
        }

        return tasks;
    }






}
