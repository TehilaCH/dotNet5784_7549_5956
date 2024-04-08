namespace BlImplementation;
using DO;
using BO;
using System.Security.Cryptography;
using DalApi;
using System.Threading.Tasks;
using BlApi;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualBasic;

internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;//**

    private readonly Bl _bl;//**
    internal TaskImplementation(Bl bl) => _bl = bl;

    /// <summary>
    ///A function that tries to create a task if it meets the correctness tests otherwise throws an exception 
    ///You can create a task only in the project planning phase
    /// </summary>
    /// <param name="boTask"></param>
    /// <returns></returns>
    /// <exception cref="BlInvalidValueException"></exception>
    public int Creat(BO.Task boTask)
    {
       
        DateTime? date = _dal.Schedule.getStartProjectDate();
        if(date != null) //execution Stage
            throw new BlInvalidValueException("Project is at execution stage, cannot create new task.");

        if (boTask.NickName == null ||boTask.PlannedDateStartWork != null || boTask.Engineer != null
             || boTask.StartDateTask != null || boTask.Deadline != null || boTask.EndDate != null)//data integrity
        {
            throw new BlInvalidValueException("The Task data is invalid");
        }
            

        DO.Task doTask = new DO.Task
        {
            Nickname = boTask.NickName,
            Description = boTask.Description,
            CreatTaskDate = _bl.Clock, 
            PlannedDateStartWork = boTask.PlannedDateStartWork,
            StartDateTask = boTask.StartDateTask,
            TimeRequired = boTask.TimeRequired,
            Deadline = boTask.Deadline,
            EndDate = boTask.EndDate,
            Product = boTask.Product,
            Remarks = boTask.Remarks,
            TaskLave = (DO.EngineerLevel?)boTask.TaskLave

        };

        int id = _dal.Task.Create(doTask);//trying to create


        if (boTask.Dependencies != null)
        {    //creates dependency
            var dependenciesToAdd = boTask.Dependencies.Select(dependency => new Dependence
            {
                IdPendingTask = id,
                IdPreviousTask = dependency.Id
            });

            foreach (var dependence in dependenciesToAdd)
            {
                _dal.Dependence.Create(dependence);
            }

        }

        return id;

    }
    /// <summary>
    /// A function that deletes a task only in the development phase and only if it has no tasks that depend on it
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BlInvalidValueException"></exception>
    /// <exception cref="BlDoesNotExistException"></exception>
    public void Delete(int id)
    {

        DateTime? date = _dal.Schedule.getStartProjectDate();
        if (date != null) //execution Stage
        {
            throw new BlInvalidValueException($"can't delete this task {id} you in execution Stage ");
        }

        var dependences = _dal.Dependence.ReadAll();
        if(dependences != null)//If there are dependencies
        {
            //return all tasks that I Previous them
            List<DO.Dependence> result = (from d in dependences
                                         where d.IdPreviousTask == id
                                         select d).ToList();
            if (result.Count!=0)//There are tasks that I Previous ,so must not delete 
            {
                throw new BlInvalidValueException($"can't delete this task {id} tasks depend on it");
            }
        }
        
        
        try
        {   
            _dal.Task.Delete(id);//Trying to delete
        }
        catch (DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException("Task DoesNot exists", ex);

        }

    }
    /// <summary>
    /// A function that returns an Task 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>
    public BO.Task Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);//call function from data 
        if (doTask == null)//Does not exist
            throw new BlDoesNotExistException($"Task with ID={id} does Not exist");

        return new BO.Task()
        {
            Id = doTask.TaskId,
            NickName = doTask.Nickname,
            Description = doTask.Description,
            CreatTaskDate = doTask.CreatTaskDate,
            PlannedDateStartWork = doTask.PlannedDateStartWork,
            StartDateTask = doTask.StartDateTask,
            TimeRequired = doTask.TimeRequired,
            Deadline = CalculationOfDeadline(doTask),
            EndDate = doTask.EndDate,
            Product = doTask.Product,
            Remarks = doTask.Remarks,
            TaskLave = (BO.EngineerLevel?)doTask.TaskLave,
            Status = stat(doTask),
            Engineer= EngineerToTask(doTask),
            Dependencies=ReDependent(doTask)

        };

    }
    /// <summary>
    /// A function that returns a filterable list of tasks
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        if (filter != null)//with filtering
        {
            return (from DO.Task doTask in _dal.Task.ReadAll()
                    let task = new BO.Task
                    {
                        Id = doTask.TaskId,
                        NickName = doTask.Nickname,
                        Description = doTask.Description,
                        CreatTaskDate = doTask.CreatTaskDate,
                        TimeRequired = doTask.TimeRequired,
                        PlannedDateStartWork = doTask.PlannedDateStartWork,
                        StartDateTask = doTask.StartDateTask,
                        Deadline = CalculationOfDeadline(doTask),
                        EndDate = doTask.EndDate,
                        Product = doTask.Product,
                        Remarks = doTask.Remarks,
                        TaskLave = (BO.EngineerLevel?)doTask.TaskLave,
                        Status = stat(doTask),
                        Engineer = EngineerToTask(doTask),
                        Dependencies = ReDependent(doTask)


                    }
                    where filter(task)
                    select task).ToList();
        }

        return (from DO.Task doTask in _dal.Task.ReadAll()//Returns a copy of the tasks
                select new BO.Task
                {
                    Id = doTask.TaskId,
                    NickName = doTask.Nickname,
                    Description = doTask.Description,
                    CreatTaskDate = doTask.CreatTaskDate,
                    TimeRequired = doTask.TimeRequired,
                    PlannedDateStartWork = doTask.PlannedDateStartWork,
                    StartDateTask = doTask.StartDateTask,
                    Deadline = CalculationOfDeadline(doTask),
                    EndDate = doTask.EndDate,
                    Product = doTask.Product,
                    Remarks = doTask.Remarks,
                    TaskLave = (BO.EngineerLevel?)doTask.TaskLave,
                    Status = stat(doTask),
                    Engineer = EngineerToTask(doTask),
                    Dependencies = ReDependent(doTask)

                }).ToList();
    }

    /// <summary>
    /// A function that updates a task according to the stage the project is in
    /// </summary>
    /// <param name="boTask"></param>
    /// <exception cref="BlInvalidValueException"></exception>
    /// <exception cref="BlDoesNotExistException"></exception>
    public void Update(BO.Task boTask)
    {
        if (boTask.NickName == null)
        {
            throw new BlInvalidValueException("The Task data is invalid");
        }

        DO.Task? doTask = _dal.Task.Read(boTask.Id);
        if (doTask == null) //Checks an existing task
            throw new BlDoesNotExistException($"Task with ID={boTask.Id} does Not exist");
        bool is_CorrectLevel = LevelOfTask(doTask.TaskLave, boTask.TaskLave);
        if(is_CorrectLevel==false)
        {
            throw new BlInvalidValueException("A task level may not be updated to a lower level");

        }
        DateTime? date = _dal.Schedule.getStartProjectDate();
        if (date == null) //planing Stage
        {
            if (boTask.Engineer.Id != null)
                throw new BlInvalidValueException("Task assignments are not possible in the planning stage");

            if (boTask.PlannedDateStartWork != null 
             || boTask.StartDateTask != null || boTask.Deadline != null || boTask.EndDate != null)
            {
                throw new BlInvalidValueException("The Task data is invalid");
            }


            //delete dependency list
            var Dep = _dal.Dependence.ReadAll()
                  .Where(d => d.IdPendingTask == boTask.Id)
                  .ToList();

            foreach (var dependence in Dep)
            {
                _dal.Dependence.Delete(dependence.IdNum);
            }

            //Update the list
            if (boTask.Dependencies!=null)
            {
                var dependenciesToAdd = boTask.Dependencies.Select(dependency => new Dependence
                {
                    IdPendingTask = boTask.Id,
                    IdPreviousTask = dependency.Id
                });

                foreach (var dependence in dependenciesToAdd)
                {
                    _dal.Dependence.Create(dependence);
                }
            }
           

        }
       
        if (date != null) //execution Stage
        {
            //checks if we have updated the list of dependencies or we have added or deleted dependencies
            List<TaskInList>? d = ReDependent(doTask);

            var Matched = (from dep1 in d
                           where !boTask.Dependencies!.Any(dep2 => dep1.Id == dep2.Id)
                           select dep1).ToList();

            if (Matched.Count!=0)//There is something missing or found from the original dependent list
            {
               
                throw new BlInvalidValueException("The Task data is invalid ban updat Dependencies in execution Stage ");
            }
            //Tests of the execution Stage​
            if (doTask.StartDateTask != boTask.StartDateTask || doTask.TimeRequired != boTask.TimeRequired
                || doTask.PlannedDateStartWork != boTask.PlannedDateStartWork || doTask.CreatTaskDate != boTask.CreatTaskDate)

            {
                throw new BlInvalidValueException("The Task data is invalid");
            }
            if (boTask.Engineer.Id !=null)//Checking the correctness of assigning an engineer to the task
            {
                if(doTask.EngineerIdToTask != 0 && doTask.EngineerIdToTask != null)//assigned to task already an engineer
                {
                    if(doTask.EngineerIdToTask != boTask.Engineer.Id)//Checking if he hasn't changed an engineer
                    {
                        throw new BlInvalidValueException("The Task data is invalid-Changing engineer assignment is not allowed");
                    }
                    
                }
                 
                DO.Engineer? engineer = _dal.Engineer.Read((int)boTask.Engineer.Id!)!;//Checks if the assigned engineer exists​
                if (engineer == null)
                    throw new BlInvalidValueException("The Task data is invalid-Engineer for assignment does not exist");
               // An engineer is assigned to another task
                List<DO.Task?> task = (from t in _dal.Task.ReadAll()
                                       where t.EngineerIdToTask == boTask.Engineer.Id
                                       select t).ToList();

                if (task.Count != 0)//If the engineer is doing another task
                {
                    //Checking if all the tasks he did are finished
                    var Tasks = (from t in task
                                 where t.EndDate == null
                                 select t).ToList();
                    if (Tasks.Count!=0)
                        throw new BlInvalidValueException("The Task data is invalid-The engineer is assigned to the task");
                }
                
                bool is_good=LevelOfTask((DO.EngineerLevel)boTask.TaskLave!,(BO.EngineerLevel)engineer.EngineerLevel!);
                //if (boTask.TaskLave != (BO.EngineerLevel)engineer.EngineerLevel!)
                if(is_good==false)
                    throw new BlInvalidValueException("The Task data is invalid-different levels");

                if(boTask.Dependencies.Count!=0)//exists for task dependencies
                {
                    var incompleteTasks = (from t in boTask.Dependencies//Previous task not completed
                                           where t.Status != Status.Done
                                           select t).ToList();

                    if (incompleteTasks.Any())
                        throw new BlInvalidValueException("It is not possible to assign an existing previous task that has not been completed");

                    
                     
                }


            }

        }
        DO.Task dTask = new DO.Task
        {
            TaskId = boTask.Id,
            Nickname = boTask.NickName,
            Description = boTask.Description,
            CreatTaskDate = boTask.CreatTaskDate,
            PlannedDateStartWork = boTask.PlannedDateStartWork,
            StartDateTask = boTask.StartDateTask,
            TimeRequired = boTask.TimeRequired,
            Deadline = boTask.Deadline,
            EndDate = boTask.EndDate,
            Product = boTask.Product,
            Remarks = boTask.Remarks,
            TaskLave = (DO.EngineerLevel?)boTask.TaskLave,
            EngineerIdToTask = boTask.Engineer?.Id ?? null
        };

        try
        {
            _dal.Task.Update(dTask);
        }
        catch (DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"Task with ID={boTask.Id} does not exist", ex);
        }
        

    }
    /// <summary>
    /// A function that updates a scheduled date for a task
    /// </summary>
    /// <param name="id"></param>
    /// <param name="date"></param>
    /// <exception cref="BlInvalidValueException"></exception>
    public void UpdateStartDate(int id, DateTime date)//Updates a planned date for the start of a task
    {
        DO.Task? task = _dal.Task.Read(id);//Checks if it exists
        if (task == null)//if Does not exist
            throw new BlDoesNotExistException($"Student with ID={id} does Not exist");
        //Returns a list of previous tasks
        var dependent = _dal.Dependence.ReadAll();
        var result = (from d in dependent
                      where d.IdPendingTask == id
                      select _dal.Task.Read((int)d.IdPreviousTask!)).ToList();


        DateTime? startProjectDate = _dal.Schedule.getStartProjectDate();
        if (result.Count == 0 || result== null)
        {
            if (startProjectDate == null || date < startProjectDate)
                throw new BlInvalidValueException("Project date does not exist or date for invalid update");
        }

        else
        {
            bool flag = result.Any(t => t.PlannedDateStartWork == null);
            if (flag == true)
            {
                throw new BlInvalidValueException("There is no planned start date for the dependent tasks");
            }
           
            bool flag2 = result.Any(t => t.PlannedDateStartWork + t.TimeRequired > date);
            if (flag2 == true)
            {
                throw new BlInvalidValueException("The update date is before the Deadline of her previous assignments");
            }
        }


        
        DO.Task doTask = new DO.Task
        {
            TaskId = task.TaskId,
            Nickname = task.Nickname,
            Description = task.Description,
            CreatTaskDate = task.CreatTaskDate,
            PlannedDateStartWork = date,
            StartDateTask = task.StartDateTask,
            TimeRequired = task.TimeRequired,
            Deadline = task.Deadline,
            EndDate = task.EndDate,
            Product = task.Product,
            Remarks = task.Remarks,
            TaskLave = (DO.EngineerLevel?)task.TaskLave,
            EngineerIdToTask = task.EngineerIdToTask ?? null

        };
        _dal.Task.Update(doTask);
    }

    /// <summary>
    /// Helper function that calculates task status
    /// </summary>
    /// <param name="boTask"></param>
    /// <returns></returns>
    public Status stat(DO.Task boTask)
    {
        if (boTask.EndDate != null)//if There is an end date
        {
            return Status.Done;
        }
        if (boTask.PlannedDateStartWork != null && boTask.StartDateTask == null)
        {
            return Status.Scheduled;
        }
        if (boTask.StartDateTask != null && boTask.EndDate == null)
        {
            return Status.OnTrack;
        }
        return Status.Unscheduled;

    }
    /// <summary>
    /// A helper function that calculates Deadline
    /// </summary>
    /// <param name="doTask"></param>
    /// <returns></returns>
    public DateTime? CalculationOfDeadline(DO.Task doTask)
    {
        DateTime? result;

        if (doTask.StartDateTask > doTask.PlannedDateStartWork)
        {
            result = doTask.StartDateTask + doTask.TimeRequired;

        }
        else
        {
            result = doTask.PlannedDateStartWork + doTask.TimeRequired;
        }

        return result;
    }
    /// <summary>
    /// A helper function that returns an engineer assigned to the task if one exists
    /// </summary>
    /// <param name="doTask"></param>
    /// <returns></returns>
    public EngineerInTask? EngineerToTask(DO.Task doTask)
    {
        if (doTask.EngineerIdToTask != null)//If there is an engineer assigned to the task
        {
            DO.Engineer? engineer = _dal.Engineer.Read((int)doTask.EngineerIdToTask);

            if (engineer != null)
            {
                EngineerInTask engineerInTask = new EngineerInTask { Id = engineer.IdNum, Name = engineer.Name };
                return engineerInTask;
            }
            return null;
        }
        return null;

    }
    /// <summary>
    /// A helper function that returns the entire list of dependencies of a task
    /// </summary>
    /// <param name="doTask"></param>
    /// <returns></returns>
    public List<TaskInList>? ReDependent(DO.Task doTask)
    {
        var depnd = _dal.Dependence.ReadAll();

        var dependency = depnd//A list of previous of this task
            .Where(d => d.IdPendingTask == doTask.TaskId)
            .Select(d => d.IdPreviousTask)
            .ToList();

        List<BO.TaskInList> taskList = new List<BO.TaskInList>();

        foreach (var dep in dependency)
        {
            var task = _dal.Task.Read((int)dep);
            if (task != null) //If the task is in the data
            {
                taskList.Add(new BO.TaskInList//Adds to the list
                {
                    Id = task.TaskId,
                    NickName = task.Nickname,
                    Description = task.Description,
                    Status = stat(task)
                });
            }
           
        }

        return taskList;
    }
    /// <summary>
    /// A function that deletes tasks and dependencies
    /// </summary>
    public void clear()
    {
        _dal.Task.clear();
        _dal.Dependence.clear();
    }
    /// <summary>
    /// Returns a copy of the task list from type TaskInList
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<TaskInList> readAll(Func<BO.TaskInList, bool>? filter = null)
    {
        if (filter != null)
        {
            return (from DO.Task doTask in _dal.Task.ReadAll()
                    let task = new BO.TaskInList
                    {
                        Id = doTask.TaskId,
                        NickName = doTask.Nickname,
                        Description = doTask.Description,
                        Status = stat(doTask),
                      
                    }
                    where filter(task)
                    select task).ToList();
        }

        return (from DO.Task doTask in _dal.Task.ReadAll()
                select new BO.TaskInList
                {
                    Id = doTask.TaskId,
                    NickName = doTask.Nickname,
                    Description = doTask.Description,
                    Status = stat(doTask)
                   
                }).ToList();
       
    }

    public void UpdateStartAndEndDate(DateTime? dateS, DateTime? dateE, int idT)
    {
        DO.Task task =_dal.Task.Read(idT);
        if (task == null)
            throw new BlDoesNotExistException($"Task with ID={idT} does Not exist");

        if (dateS !=null && dateS != DateTime.MinValue)
        {
            if (task.StartDateTask == null|| task.StartDateTask==DateTime.MinValue)
            {
                DO.Task doTask = new DO.Task
                {
                    TaskId = task.TaskId,
                    Nickname = task.Nickname,
                    Description = task.Description,
                    CreatTaskDate = task.CreatTaskDate,
                    PlannedDateStartWork = task.PlannedDateStartWork,
                    StartDateTask = dateS,
                    TimeRequired = task.TimeRequired,
                    Deadline = task.Deadline,
                    EndDate = task.EndDate,
                    Product = task.Product,
                    Remarks = task.Remarks,
                    TaskLave = (DO.EngineerLevel?)task.TaskLave,
                    EngineerIdToTask = task.EngineerIdToTask 
                };
                _dal.Task.Update(doTask);
            }
            else
                throw new BlDoesNotExistException($"Task with ID={idT} has a start Date ");

        }
       
        if (dateE != null && dateE != DateTime.MinValue)
        {
            if (task.EndDate == null || task.EndDate == DateTime.MinValue)
            {
                DO.Task doTask = new DO.Task
                {
                    TaskId = task.TaskId,
                    Nickname = task.Nickname,
                    Description = task.Description,
                    CreatTaskDate = task.CreatTaskDate,
                    PlannedDateStartWork = task.PlannedDateStartWork,
                    StartDateTask = task.StartDateTask,
                    TimeRequired = task.TimeRequired,
                    Deadline = task.Deadline,
                    EndDate = dateE,
                    Product = task.Product,
                    Remarks = task.Remarks,
                    TaskLave = (DO.EngineerLevel?)task.TaskLave,
                    EngineerIdToTask = task.EngineerIdToTask 
                };
                _dal.Task.Update(doTask);
            }
            else
                throw new BlDoesNotExistException($"Task with ID={idT} has a finish Date ");
        }

    }

    public bool LevelOfTask(DO.EngineerLevel? lev1, BO.EngineerLevel? lev2)
    {
        switch (lev1)
        {
            case DO.EngineerLevel.Advanced:
                if (lev2 == BO.EngineerLevel.Beginner)
                    return false;
                else return true;
            case DO.EngineerLevel.AdvancedBeginner:
                if (lev2 == BO.EngineerLevel.Advanced || lev2 == BO.EngineerLevel.Beginner)
                    return false;
                else return true;
            case DO.EngineerLevel.Intermediate:
                if (lev2 == BO.EngineerLevel.Beginner || lev2 == BO.EngineerLevel.Advanced
                    || lev2 == BO.EngineerLevel.AdvancedBeginner)
                    return false;
                else return true;
            case DO.EngineerLevel.Expert:
                if (lev2 == BO.EngineerLevel.Beginner || lev2 == BO.EngineerLevel.Advanced
                    || lev2 == BO.EngineerLevel.AdvancedBeginner || lev2 == BO.EngineerLevel.Intermediate)
                    return false;
                else return true;

            default: return true; //In this case the case of the lowest level will enter 

        }

    }

}

