namespace BlImplementation;
using DO;
using BO;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal _dal = Factory.Get;

    public int Creat(BO.Task boTask)
    {
        //ProjectStatus lev = projectlevel();
        //if (lev == ProjectStatus.middleStage || lev == ProjectStatus.executionStage)//============
        //    throw new BlInvalidValueException("you in executionStage or middleStage and you can't Creat");

        //כשהשם שווה null לא זורק חריגה לבדוק
        if (boTask.NickName == null || boTask.Engineer != null||boTask.PlannedDateStartWork != null || boTask.Engineer != null || boTask.CreatTaskDate != null//=============
             || boTask.StartDateTask != null || boTask.Deadline != null || boTask.EndDate != null)
        {
            throw new BlInvalidValueException("The Task data is invalid");
        }
            

        DO.Task doTask = new DO.Task
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
            TaskLave = (DO.EngineerLevel?)boTask.TaskLave

        };

        int id = _dal.Task.Create(doTask);
        if (boTask.Dependencies != null)
        {
            var newDependencies = boTask.Dependencies
            .Select(d => _dal.Dependence.Create(new DO.Dependence { IdPendingTask = boTask.Id, IdPreviousTask = d.Id }));
        }

        return id;

    }

    public void Delete(int id)
    {
        var dependences = _dal.Dependence.ReadAll();
        var result = dependences.Where(d => d.IdPreviousTask == id).ToList();
        if (result != null)
        {
            throw new BlInvalidValueException($"can't delete this task {id}");
        }
        //ProjectStatus level = projectlevel();
        //if (level == ProjectStatus.executionStage || level == ProjectStatus.middleStage)
        //{
        //    throw new NotImplementedException();
        //}
        try
        {   
            _dal.Task.Delete(id);
        }
        catch (DalDoesNotExistException ex)//לסדר שהתפוס את החריגה אם יש 
        {
            Console.WriteLine(ex);
          
        }
        
    }

    public BO.Task Read(int id)//===========
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BlDoesNotExistException($"Student with ID={id} does Not exist");

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

    public IEnumerable<BO.Task> ReadAll()//==========
    {
        return (from DO.Task doTask in _dal.Task.ReadAll()
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


    public void Update(BO.Task boTask)
    {
        if (boTask.NickName == null)
        {
            throw new BlInvalidValueException("The Task data is invalid");
        }

        DO.Task? doTask = _dal.Task.Read(boTask.Id);
        if (doTask == null)
            throw new BlDoesNotExistException($"Student with ID={boTask.Id} does Not exist");

       //// ProjectStatus level = _dal.projectlevel();
       // //שלב התכנון
       // if (level == ProjectStatus.planingStage)
       //     if (boTask.PlannedDateStartWork != null || boTask.Engineer != null || boTask.CreatTaskDate != null
       //      || boTask.StartDateTask != null || boTask.Deadline != null || boTask.EndDate != null)
       //         throw new BlInvalidValueException("The Task data is invalid");
             
         
        if (boTask.Dependencies != null)
        {    //מעדכן רשימת תלויות 
         
            List<TaskInList>? dep = ReDependent(doTask);
            if(dep!= boTask.Dependencies)
            {
               var Dep= (from dependent in dep
                         from depn in boTask.Dependencies
                         where dependent.Id!=depn.Id
                         select dependent).ToList();


                Dep.Select(d => _dal.Dependence.Create(new DO.Dependence { IdPendingTask = boTask.Id, IdPreviousTask = d.Id }));
            }
        }

        //שלב הביצוע
        //if (level == ProjectStatus.executionStage)
        //{
        //    List<TaskInList>? d = ReDependent(doTask);// לבדוק אם יש צורך
        //    if (doTask.StartDateTask != boTask.StartDateTask || doTask.TimeRequired != boTask.TimeRequired
        //        || doTask.PlannedDateStartWork != boTask.PlannedDateStartWork || doTask.CreatTaskDate != boTask.CreatTaskDate
        //       || doTask.EndDate != boTask.EndDate || d != boTask.Dependencies)
        //    {
        //        throw new BlInvalidValueException("The Task data is invalid");
        //    }
        //    if (boTask.Engineer != null)//בדיקת תקינות הקצאת מהנדס למשימה
        //    {
        //        DO.Engineer engineer = _dal.Engineer.Read((int)boTask.Engineer.Id);
        //        if(engineer == null)
        //            throw new BlInvalidValueException("The Task data is invalid");

        //        List< DO.Task?> task = (from t in _dal.Task.ReadAll()
        //                        where t.EngineerIdToTask == boTask.Engineer.Id
        //                        select t).ToList();
                
        //        if(task!=null) 
        //        {
        //            var Tasks = (from t in task
        //                         where t.EndDate== null
        //                         select t).ToList();
        //            if(Tasks!=null)
        //                throw new BlInvalidValueException("The Task data is invalid");
        //        }

        //        if (boTask.TaskLave != (BO.EngineerLevel)engineer.EngineerLevel)
        //            throw new BlInvalidValueException("The Task data is invalid");


        //        var Task = (from t in _dal.Task.ReadAll()
        //                     from dependency in boTask.Dependencies
        //                     where t.TaskId == dependency.Id && t.EndDate == null
        //                     select t).ToList();

        //        if (Task!=null)
        //            throw new BlInvalidValueException("The Task data is invalid");

        //    }

        //}

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
            EngineerIdToTask = boTask.Engineer.Id
        };
        try
        {
            _dal.Task.Update(dTask);
        }
        catch (DalDoesNotExistException ex)//לסדר שהתפוס את החריגה אם יש 
        {
            Console.WriteLine(ex);
             throw new BlDoesNotExistException($"Task with ID={boTask.Id} does not exists");

        }


    }
    public void UpdateStartDate(int id, DateTime date)
    {
        
        var dependent = _dal.Dependence.ReadAll();
        var result = (from d in dependent
                      where d.IdPendingTask == id 
                      select _dal.Task.Read((int)d.IdPreviousTask!)).ToList();
        if (result==null) 
        {
            if (_dal.StartProjectDate == null || date < _dal.StartProjectDate)
                throw new BlInvalidValueException("Project date does not exist or date for invalid update");
        }

        else
        {
            bool flag = result.Any(t => t.PlannedDateStartWork == null);
            if (flag == true)
            {
                throw new BlInvalidValueException("There is no planned start date for the dependent tasks");
            }
            //bool flag2 = result.Any(t => t.Deadline > date);
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

    public List<TaskInList>? ReDependent(DO.Task doTask)
    {
        var depnd = _dal.Dependence.ReadAll();

        List<TaskInList> taskDependent = depnd
            .Where(dependency => dependency?.IdPendingTask == doTask.TaskId)
            .Select(dependency => new TaskInList
            {
                Id = (int)dependency.IdPreviousTask!,
                NickName = doTask.Nickname,
                Description = doTask.Description,
                Status = stat(doTask)
            })
            .ToList();

        return taskDependent;
    }


    //public ProjectStatus projectlevel()
    //{
    //    if (_dal.StartProjectDate == null)
    //    {
    //        return ProjectStatus.planingStage;
    //    }
    //    var tasks = _dal.Task.ReadAll();
    //    bool isAllPlaning = tasks.All(t => t.PlannedDateStartWork != null);

    //    if (_dal.StartProjectDate != null && isAllPlaning == true)
    //    {
    //        return ProjectStatus.executionStage;
    //    }
    //    return ProjectStatus.middleStage;

    //}

    public void ProjectStartDateUpdate(DateTime date)
    {
        _dal.StartProjectDate = date;
    }

    public void ProjectEndDateUpdate(DateTime date)
    {
        _dal.EndProjectDate= date;
    }

}

