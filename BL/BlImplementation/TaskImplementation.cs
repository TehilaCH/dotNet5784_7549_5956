namespace BlImplementation;
using DO;
using BO;
using System.Security.Cryptography;

internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
   
    /// <summary>
    ///A function that tries to create a task if it meets the correctness tests otherwise throws an exception 
    ///You can create a task only in the project planning phase
    /// </summary>
    /// <param name="boTask"></param>
    /// <returns></returns>
    /// <exception cref="BlInvalidValueException"></exception>
    public int Creat(BO.Task boTask)//=====
    {
       
        DateTime? date = _dal.Schedule.getStartProjectDate();
        if(date != null) //execution Stage
            throw new BlInvalidValueException("Project is at execution stage, cannot create new task.");

        if (boTask.NickName == null ||boTask.PlannedDateStartWork != null || boTask.Engineer != null
             || boTask.StartDateTask != null || boTask.Deadline != null || boTask.EndDate != null)
        {
            throw new BlInvalidValueException("The Task data is invalid");
        }
            

        DO.Task doTask = new DO.Task
        {
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
            TaskLave = (DO.EngineerLevel?)boTask.TaskLave

        };

        int id = _dal.Task.Create(doTask);

     
        if (boTask.Dependencies != null)
        {
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
        if(dependences != null) 
        {
            
           List<DO.Dependence> result = (from d in dependences
                                         where d.IdPreviousTask == id
                                         select d).ToList();
            if (result.Count!=0)
            {
                throw new BlInvalidValueException($"can't delete this task {id}");
            }
        }
        
        
        try
        {   
            _dal.Task.Delete(id);
        }
        catch (DalDoesNotExistException ex)//לסדר שהתפוס את החריגה אם יש 
        {
            throw new BlDoesNotExistException("Task DoesNot exists", ex);

        }

    }
    /// <summary>
    /// A function that returns an object with an option according to filtering
    /// </summary>
    /// <param name="id"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>
    public BO.Task Read(int id, Func<DO.Task, bool>? filter = null)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BlDoesNotExistException($"Student with ID={id} does Not exist");

        //by filter
        if (filter != null && !filter(doTask))
            throw new BlDoesNotExistException($"No task found matching the provided criteria");


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
    public IEnumerable<BO.Task> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        return (from DO.Task doTask in _dal.Task.ReadAll()
                where filter == null || filter(doTask)//
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
    public void Update(BO.Task boTask)//=========
    {
        if (boTask.NickName == null)
        {
            throw new BlInvalidValueException("The Task data is invalid");
        }

        DO.Task? doTask = _dal.Task.Read(boTask.Id);
        if (doTask == null)
            throw new BlDoesNotExistException($"Student with ID={boTask.Id} does Not exist");

        DateTime? date = _dal.Schedule.getStartProjectDate();
        if (date == null) //planing Stage
        {
            
            if (boTask.PlannedDateStartWork != null || boTask.Engineer != null 
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
            //checks when we have updated the list of dependencies i.e. we have added or deleted dependencies
            List<TaskInList>? d = ReDependent(doTask);
          

            var Matched = (from dep1 in d
                              from dep2 in boTask.Dependencies!
                              where dep1.Id != dep2.Id
                              select dep1).ToList();

            if (Matched.Count!=0)
            {
               
                throw new BlInvalidValueException("The Task data is invalid ban updat Dependencies in execution Stage ");
            }
           

            if (doTask.StartDateTask != boTask.StartDateTask || doTask.TimeRequired != boTask.TimeRequired
                || doTask.PlannedDateStartWork != boTask.PlannedDateStartWork || doTask.CreatTaskDate != boTask.CreatTaskDate
               || doTask.EndDate != boTask.EndDate )
            {
                throw new BlInvalidValueException("The Task data is invalid");
            }
            if (boTask.Engineer != null)//Checking the correctness of assigning an engineer to the task
            {
                DO.Engineer? engineer = _dal.Engineer.Read((int)boTask.Engineer.Id!)!;
                if (engineer == null)
                    throw new BlInvalidValueException("The Task data is invalid");

                List<DO.Task?> task = (from t in _dal.Task.ReadAll()
                                       where t.EngineerIdToTask == boTask.Engineer.Id
                                       select t).ToList();

                if (task.Count != 0)//!= null
                {
                    var Tasks = (from t in task
                                 where t.EndDate == null
                                 select t).ToList();
                    if (Tasks.Count!=0)
                        throw new BlInvalidValueException("The Task data is invalid");
                }

                if (boTask.TaskLave != (BO.EngineerLevel)engineer.EngineerLevel!)
                    throw new BlInvalidValueException("The Task data is invalid");


                var Task = (from t in _dal.Task.ReadAll()
                            from dependency in boTask.Dependencies!
                            where t.TaskId == dependency.Id && t.EndDate == null
                            select t).ToList();

                if (Task.Count != 0)//!= null
                    throw new BlInvalidValueException("The Task data is invalid");

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
    public void UpdateStartDate(int id, DateTime date)
    {
        
        var dependent = _dal.Dependence.ReadAll();
        var result = (from d in dependent
                      where d.IdPendingTask == id 
                      select _dal.Task.Read((int)d.IdPreviousTask!)).ToList();
        DateTime? startProjectDate = _dal.Schedule.getStartProjectDate();
        if (result.Count == 0)
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
                throw new BlInvalidValueException("The update date is before the Deadline");
            }
        }


        DO.Task? task =_dal.Task.Read(id);
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
            TaskLave = (DO.EngineerLevel?)task.TaskLave

        };
        _dal.Task.Update(task);
    }

    /// <summary>
    /// Helper function that calculates task status
    /// </summary>
    /// <param name="boTask"></param>
    /// <returns></returns>
    public Status stat(DO.Task boTask)
    {
        if (boTask.EndDate != null)
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
        if (doTask.EngineerIdToTask != null)
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

        List<TaskInList> taskDependent = depnd
            .Where(dependency => dependency?.IdPendingTask == doTask.TaskId)
            .Select(dependency => new TaskInList
            {
                Id = (int)dependency!.IdPreviousTask!,
                NickName = doTask.Nickname,
                Description = doTask.Description,
                Status = stat(doTask)
            })
            .ToList();

        return taskDependent;
    }
    /// <summary>
    /// A function that deletes tasks and dependencies
    /// </summary>
    public void clear()
    {
        _dal.Task.clear();
        _dal.Dependence.clear();
    }


   
}

