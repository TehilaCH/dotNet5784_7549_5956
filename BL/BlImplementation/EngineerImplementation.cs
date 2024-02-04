namespace BlImplementation;
using BO;
using DO;
using System.Collections.Generic;
using System.Data;

internal class EngineerImplementation : BlApi.IEngineer
{
    private DalApi.IDal _dal = Factory.Get;

    public int Creat(BO.Engineer boEngineer)
    {
        if (boEngineer.Id < 1 || boEngineer.Cost < 0 || boEngineer.Name == null || boEngineer.Email == null)
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
            Console.WriteLine(ex);
            throw new BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} already exists");
        }

       
    }

    public void Delete(int id)
    {
        DO.Task? taskForEngineer = _dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerIdToTask == id);

        if (taskForEngineer != null)
        { 
            if(taskForEngineer.EndDate!=null ||
                (taskForEngineer.EndDate==null && taskForEngineer.StartDateTask!=null))
                   throw new BlInvalidValueException("One or more parameters are incorrect");
        }

        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DalDoesNotExistException ex)
        {
            Console.WriteLine(ex);
            throw new BlDoesNotExistException($"Engineer with ID={id} does not exists");
        }
       
    }

    public BO.Engineer Read(int id)
    {

        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BlDoesNotExistException($"Engineer with ID={id} does Not exist");

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

    public IEnumerable<BO.Engineer> ReadAll()
    {
        return(from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
               let taskForEngineer = _dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerIdToTask == doEngineer.IdNum)
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
        DO.Engineer? Engineer = _dal.Engineer.Read(boEngineer.Id);
        bool islevel = LevelOfEngineer(Engineer.EngineerLevel, boEngineer.Level);
       
        if (boEngineer.Cost < 0 || boEngineer.Name == null || boEngineer.Email == null || islevel==false)
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

            if (updatedTask != null)
            {
                _dal.Task.Update(updatedTask);
            }
        }
        catch (DalDoesNotExistException ex)
        {
            Console.WriteLine(ex);
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
}




