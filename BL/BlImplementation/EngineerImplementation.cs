namespace BlImplementation;
using BO;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Data;
using System.Linq;

internal class EngineerImplementation : BlApi.IEngineer
{
    private DalApi.IDal _dal = Factory.Get;
    Bl blInstance = new Bl();

    public int Creat(BO.Engineer boEngineer)
    {
        
        if (boEngineer.Id < 1  || boEngineer.Cost < 0 || boEngineer.Name == null 
            || IsValidEmail(boEngineer.Email!)==false || boEngineer.Task!=null)
        {
            throw new BlInvalidValueException("One or more parameters are incorrect");
        }
        DO.Engineer doEngineer = new DO.Engineer(
            boEngineer.Id,
            boEngineer.Name,
            boEngineer.Email,
            (DO.EngineerLevel?)boEngineer.Level,
            boEngineer.Cost);

        try
        {
            int idEngineer = _dal.Engineer.Create(doEngineer);
            return idEngineer;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BlAlreadyExistsException("Engineer already exists", ex);

        }

       
    }

    public void Delete(int id)
    {
        DO.Task? taskForEngineer = _dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerIdToTask == id);

        if (taskForEngineer != null)
        { 
            throw new BlInvalidValueException("Cannot delete - engineer assigned to task");
        }

        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException("Engineer DoesNot exists", ex);

        }

    }

    public BO.Engineer Read(int id, Func<DO.Engineer, bool>? filter = null)
    {

        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BlDoesNotExistException($"Engineer with ID={id} does Not exist");

        if (filter != null && !filter(doEngineer))
            throw new BlDoesNotExistException($"No task found matching the provided criteria");


        DO.Task? taskForEngineer = _dal.Task.ReadAll()
            .FirstOrDefault(task => task?.EngineerIdToTask == doEngineer.IdNum);
        return new BO.Engineer
        {
            Id = doEngineer.IdNum,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Cost = doEngineer.CostPerHour,
            Level = (BO.EngineerLevel?)doEngineer.EngineerLevel,
            Task = taskForEngineer != null ? new BO.TaskInEngineer
            {
                Id = taskForEngineer.TaskId,
                NickName = taskForEngineer.Nickname
            } : null

        };
    }

    public IEnumerable<BO.Engineer> ReadAll(Func<DO.Engineer, bool>? filter = null)
    {
        return(from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
               let taskForEngineer = _dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerIdToTask == doEngineer.IdNum)
               where filter == null || filter(doEngineer)//
               select new BO.Engineer
               {
                   Id = doEngineer.IdNum,
                   Name = doEngineer.Name,
                   Email = doEngineer.Email,
                   Cost = doEngineer.CostPerHour,
                   Level = (BO.EngineerLevel?)doEngineer.EngineerLevel,
                   Task = taskForEngineer != null ? new BO.TaskInEngineer
                   {
                       Id = taskForEngineer.TaskId,
                       NickName = taskForEngineer.Nickname
                   } : null

               }).ToList();
    }

    public void Update(BO.Engineer boEngineer)
    {
        //**
        int levP = 2;
        //**
        ProjectStatus projectLevel = blInstance.projectlevel();
       // if (projectLevel== ProjectStatus.planingStage)
        if(levP==1)
        {
            if (boEngineer.Task != null)
               throw new BlInvalidValueException("One or more parameters are incorrect");

        }

        DO.Engineer? Engineer = _dal.Engineer.Read(boEngineer.Id);
        bool islevel = LevelOfEngineer(Engineer.EngineerLevel, boEngineer.Level);
        bool flag= checkEngineerToTask(boEngineer);

        if (boEngineer.Cost < 0 || boEngineer.Name == null || IsValidEmail(boEngineer.Email!) == false
            || islevel == false || flag == false)
        {
            throw new BlInvalidValueException("One or more parameters are incorrect");
        }


        DO.Engineer doEngineer= new DO.Engineer
        {
            IdNum = boEngineer.Id,
            Name =boEngineer.Name,
            Email = boEngineer.Email,
            CostPerHour = boEngineer.Cost,
            EngineerLevel = (DO.EngineerLevel?)boEngineer.Level
        };
        try 
        {
            _dal.Engineer.Update(doEngineer);

            if (boEngineer.Task != null)
            {
                var taskIdToUpdate = boEngineer.Task.Id;

                var updatedTask =
                    (from t in _dal.Task.ReadAll()
                     where t.TaskId == taskIdToUpdate
                     select new DO.Task
                     {
                         TaskId = t.TaskId,
                         Nickname = t.Nickname,
                         Description = t.Description,
                         CreatTaskDate = t.CreatTaskDate,
                         PlannedDateStartWork = t.PlannedDateStartWork,
                         StartDateTask = t.StartDateTask,
                         TimeRequired = t.TimeRequired,
                         Deadline = t.Deadline,
                         EndDate = t.EndDate,
                         Product = t.Product,
                         Remarks = t.Remarks,
                         TaskLave = t.TaskLave,
                         EngineerIdToTask = doEngineer.IdNum
                     })
                    .FirstOrDefault();
                _dal.Task.Update(updatedTask);
            }
        }
        catch (DalDoesNotExistException ex)
        {
           
            throw new BlDoesNotExistException($"Engineer with ID={boEngineer.Id} does not exists");
        }

    }

   

    public bool LevelOfEngineer(DO.EngineerLevel? lev1, BO.EngineerLevel? lev2)
    {
        switch(lev1)
        {
          case DO.EngineerLevel.Advanced:
                if(lev2== BO.EngineerLevel.Beginner)
                     return false;
                else return true;
          case DO.EngineerLevel.AdvancedBeginner: 
                if ( lev2 == BO.EngineerLevel.Advanced || lev2 == BO.EngineerLevel.Beginner)
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

            default: return true; //במקרה זה יכנס המקרה של הרמה הנמוכה ביותר 
        
        }
       
    }
    bool IsValidEmail(string email)
    {
        if (email == null) return false;
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false;
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }
    bool checkEngineerToTask(BO.Engineer boEngineer) 
    {
        if (boEngineer.Task == null)
            return true;
        DO.Task? Task1 = _dal.Task.Read((int)boEngineer.Task.Id!);
        if(Task1 == null )
            return false;
        
        if (Task1.EngineerIdToTask == 0 && (boEngineer.Task.Id == Task1.TaskId))
                if(Task1.TaskLave != null&& boEngineer.Level!=null)
                {
                    if (Task1.TaskLave == (DO.EngineerLevel)boEngineer.Level)
                    { 
                         List<DO.Dependence>? dep = (from dependency in _dal.Dependence.ReadAll()
                                      where dependency.IdPendingTask == boEngineer.Task.Id
                                      select dependency).ToList();

                         var tasks = (from t in _dal.Task.ReadAll()
                                      where dep.Any(d => d.IdPendingTask == t.TaskId) && t.EndDate == null
                                      select t).ToList();
                          if (tasks.Count==0)
                          {
                              return true;
                          }

                    }
                    
                }
            
        return false;

    }
   public List<BO.Engineer> OrderEngineers () 
    {
         
        return (from e in _dal.Engineer.ReadAll()
                let taskForEngineer = _dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerIdToTask == e.IdNum)
                orderby e.Name

               select new BO.Engineer
               {
                   Id = e.IdNum,
                   Name = e.Name,
                   Email = e.Email,
                   Cost = e.CostPerHour,
                   Level = (BO.EngineerLevel?)e.EngineerLevel,
                   Task = taskForEngineer != null ? new BO.TaskInEngineer
                   {
                       Id = taskForEngineer.TaskId,
                       NickName = taskForEngineer.Nickname
                   } : null

               }).ToList();

        
    }
    public Dictionary<BO.EngineerLevel, List<BO.Engineer>> GroupByEngineerLevel()
    {
        var engineersGrouped = _dal.Engineer.ReadAll()
            .GroupBy(engineer => (BO.EngineerLevel)engineer.EngineerLevel)
            .ToDictionary(
                group => group.Key,
                group => group.Select(doEngineer =>
                {
                    var taskForEngineer = GetTaskForEngineer(doEngineer);
                    return new BO.Engineer
                    {
                        Id = doEngineer.IdNum,
                        Name = doEngineer.Name,
                        Email = doEngineer.Email,
                        Cost = doEngineer.CostPerHour,
                        Level = (BO.EngineerLevel)doEngineer.EngineerLevel,
                        Task = taskForEngineer
                    };
                }).ToList()
            );

        return engineersGrouped;
    }

    private BO.TaskInEngineer? GetTaskForEngineer(DO.Engineer doEngineer)
    {
        var taskForEngineer = _dal.Task.ReadAll()
            .FirstOrDefault(task => task?.EngineerIdToTask == doEngineer.IdNum);

        if (taskForEngineer != null)
        {
            return new BO.TaskInEngineer
            {
                Id = taskForEngineer.TaskId,
                NickName = taskForEngineer.Nickname
            };
        }
        else
        {
            return null;
        }
    }






    public void clear()
    {
        _dal.Engineer.clear();
    }

  
}




