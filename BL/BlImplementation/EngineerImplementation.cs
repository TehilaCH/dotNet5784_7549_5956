﻿namespace BlImplementation;
using BO;
using DO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;

internal class EngineerImplementation : BlApi.IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
  
    /// <summary>
    /// If the data is correct - you will make an attempt to request an addition to the data layer,
    /// otherwise you will throw an exception
    /// </summary>
    /// <param name="boEngineer"></param>
    /// <returns></returns>
    /// <exception cref="BlInvalidValueException"></exception>
    /// <exception cref="BlAlreadyExistsException"></exception>
    public int Creat(BO.Engineer boEngineer)
    {
        
        if (boEngineer.Id < 1  || boEngineer.Cost < 0 || boEngineer.Name == null 
            || IsValidEmail(boEngineer.Email!)==false || boEngineer.Task!=null)//Integrity checks
        {
            throw new BlInvalidValueException("One or more parameters are incorrect");
        }

            DO.Engineer doEngineer = new DO.Engineer(//Creates an object
            boEngineer.Id,
            boEngineer.Name,
            boEngineer.Email,
            (DO.EngineerLevel?)boEngineer.Level,
            boEngineer.Cost);

        try
        {
            int idEngineer = _dal.Engineer.Create(doEngineer);//trying to create
            return idEngineer;//Returns the created id
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BlAlreadyExistsException("Engineer already exists", ex);

        }

       
    }
    /// <summary>
    /// Deleting an engineer on the condition that he is not assigned a task
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BlInvalidValueException"></exception>
    /// <exception cref="BlDoesNotExistException"></exception>
    public void Delete(int id)
    {
        DO.Task? taskForEngineer = _dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerIdToTask == id);//Checks if the engineer is assigned
                                                                                                            //to the task

        if (taskForEngineer != null)//If assigned throws an exception
        { 
            throw new BlInvalidValueException("Cannot delete - engineer assigned to task");
        }

        try
        {
            _dal.Engineer.Delete(id);//Trying to delete an engineer
        }
        catch (DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException("Engineer DoesNot exists", ex);

        }

    }
    /// <summary>
    /// Returns an engineer by id if it exists otherwise throws an exception 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>
    public BO.Engineer Read(int id)
    {

        DO.Engineer? doEngineer = _dal.Engineer.Read(id);//calls a function from the data
        if (doEngineer == null) //If no engineer exists
            throw new BlDoesNotExistException($"Engineer with ID={id} does Not exist");

        return new BO.Engineer
        {
            Id = doEngineer.IdNum,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Cost = doEngineer.CostPerHour,
            Level = (BO.EngineerLevel?)doEngineer.EngineerLevel,
            Task = GetEngineerTask(doEngineer.IdNum)//A function that returns the task the engineer
                                                    //is assigned to
        };
    }
    /// <summary>
    /// Returns a list of engineers and can also be filtered
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        if(filter != null)//with a filter
        {
            return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                    let engineer= new BO.Engineer
                    {
                        Id = doEngineer.IdNum,
                        Name = doEngineer.Name,
                        Email = doEngineer.Email,
                        Cost = doEngineer.CostPerHour,
                        Level = (BO.EngineerLevel?)doEngineer.EngineerLevel,
                        Task = GetEngineerTask(doEngineer.IdNum)//A function that returns the task the engineer
                                                                //is assigned to

                    }
                    where filter(engineer)//Checking if the filtering is completed
                    select engineer).ToList();
        }
        return(from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
               let engineer = new BO.Engineer
               {
                   Id = doEngineer.IdNum,
                   Name = doEngineer.Name,
                   Email = doEngineer.Email,
                   Cost = doEngineer.CostPerHour,
                   Level = (BO.EngineerLevel?)doEngineer.EngineerLevel,
                   Task = GetEngineerTask(doEngineer.IdNum)//A function that returns the task the engineer
                                                           //is assigned to


               }
               select engineer).ToList();
    }
    /// <summary>
    /// Halp function calculate Task In Engineer
    /// </summary>
    /// <param name="engineerId"></param>
    /// <returns></returns>
    private BO.TaskInEngineer? GetEngineerTask(int engineerId)
    {
        var taskForEngineer = _dal.Task.ReadAll()
            .Where(task => task.EngineerIdToTask == engineerId && (task.EndDate == null || task.EndDate == DateTime.MinValue))//If there is a task that the
                                                                                                                              //engineer is assigned to and its end date is empty
            .Select(task => new BO.TaskInEngineer//Creates an object of a task assigned to an engineer
            {
                Id = task.TaskId,
                NickName = task.Nickname
            })
            .FirstOrDefault();

        return taskForEngineer;
    }
    /// <summary>
    /// Updates an engineer according to the conditions of the project phase he is in
    /// </summary>
    /// <param name="boEngineer"></param>
    /// <exception cref="BlInvalidValueException"></exception>
    /// <exception cref="BlDoesNotExistException"></exception>
    public void Update(BO.Engineer boEngineer)
    {
       
        DateTime? date = _dal.Schedule.getStartProjectDate();
        if (date == null) //planing Stage
        {
            if (boEngineer.Task.Id != null)//Checking if an engineer is assigned to the task
                throw new BlInvalidValueException("planing Stage is not allowed to assign an engineer to the task");

        }

        DO.Engineer? Engineer = _dal.Engineer.Read(boEngineer.Id);
        if (Engineer == null)
            throw new BlDoesNotExistException("Engineer DoesNot exists");

        bool islevel = LevelOfEngineer(Engineer.EngineerLevel, boEngineer.Level);//Engineer level update check
        try
        {
            checkEngineerToTask(boEngineer);//Checking the assignment of an engineer to a task
        }
        catch(BlInvalidValueException ex)
        {
            throw new BlInvalidValueException($"{ex.Message}");
        }

        //bool flag= checkEngineerToTask(boEngineer);//Checking the assignment of an engineer to a task

        //if (flag == false)
        //{
        //    throw new BlInvalidValueException("Assignment not possible");
        //}
        if (islevel == false)
        {
            throw new BlInvalidValueException("It is not possible to update an engineer level to a lower level");
        }
        
        if (boEngineer.Cost < 0 || boEngineer.Name == null || IsValidEmail(boEngineer.Email!) == false)
           // || islevel == false || flag == false)//Integrity checks
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
            _dal.Engineer.Update(doEngineer);//Trying to update

            if (boEngineer.Task.Id != null)//If you assign a task to an engineer
            {
                var taskIdToUpdate = boEngineer.Task.Id;//id of assigned task

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
                _dal.Task.Update(updatedTask!);//Updates the task assigned to the engineer
            }
        }
        catch (DalDoesNotExistException ex)
        {
           
            throw new BlDoesNotExistException($"Engineer with ID={boEngineer.Id} does not exists");
        }

    }


    /// <summary>
    /// An halp function that checks whether the update of the engineer level is correct 
    /// (it is possible to change the engineer level up and not down)
    /// </summary>
    /// <param name="lev1"></param>
    /// <param name="lev2"></param>
    /// <returns></returns>
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

            default: return true; //In this case the case of the lowest level will enter 

        }
       
    }
    /// <summary>
    /// An halp function for checking the integrity of an email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    bool IsValidEmail(string email)//checking the integrity of an email
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
    /// <summary>
    /// An halp function for checking the correctness of assigning a task to an engineer
    /// </summary>
    /// <param name="boEngineer"></param>
    /// <returns></returns>
    bool checkEngineerToTask(BO.Engineer boEngineer) //Checking if the assigned is good
    {
        if (boEngineer.Task.Id == null)
            return true;
        DO.Task? Task1 = _dal.Task.Read((int)boEngineer.Task.Id!);//Checking if the task exist
        if (Task1 == null || Task1.EngineerIdToTask!=0)//Checks if it has been assigned to another engineer
            throw new BlInvalidValueException("A task to assign does not exist or an engineer is already assigned to it");
             //return false;
      
        // An engineer is assigned to another task
        List<DO.Task?> task = (from t in _dal.Task.ReadAll()
                               where t.EngineerIdToTask == boEngineer.Id
                               select t).ToList();

        if (task.Count != 0)//If the engineer is doing another task
        {
            //Checking if all the tasks he did are finished
            var Tasks = (from t in task
                         where t.EndDate == null
                         select t).ToList();
            if (Tasks.Count != 0)//There is a task he is assigned to
                throw new BlInvalidValueException("An engineer is assigned to a task");
                //return false;


            //throw new BlInvalidValueException("The Task data is invalid");
        }
       
        if (Task1.TaskLave != null&& boEngineer.Level!=null)
        {
            bool is_good = LevelOfEngineer(Task1.TaskLave,boEngineer.Level);
            //if (Task1.TaskLave == (DO.EngineerLevel)boEngineer.Level)

            if (is_good==true)
            {    //Checking If the assignment task has previous tasks
                List<DO.Dependence>? dep = (from dependency in _dal.Dependence.ReadAll()
                                            where dependency.IdPendingTask == boEngineer.Task.Id
                                            select dependency).ToList();
               
                if (dep.Count == 0)
                    return true;
                //Checking If a previous task has not finish
                var tasks = (from t in _dal.Task.ReadAll()
                             where dep.Any(d => d.IdPreviousTask == t.TaskId) && t.EndDate == null
                             select t).ToList();
                if (tasks.Count != 0)
                {
                    throw new BlInvalidValueException("previous task has not finish");
                }
                if (tasks.Count == 0)
                {
                    return true;
                }

            }

        }
        throw new BlInvalidValueException("Assignment not possible");    
        //return false;

    }
    /// <summary>
    /// A function that sorts the engineers by name
    /// </summary>
    /// <returns></returns>
    public List<BO.Engineer> OrderEngineers () 
    {
         
        return (from e in _dal.Engineer.ReadAll()
                orderby e.Name
               select new BO.Engineer
               {
                   Id = e.IdNum,
                   Name = e.Name,
                   Email = e.Email,
                   Cost = e.CostPerHour,
                   Level = (BO.EngineerLevel?)e.EngineerLevel,
                   Task = GetEngineerTask(e.IdNum)
               }).ToList();

        
    }
    /// <summary>
    /// A function that groups engineers according to their level of experience
    /// </summary>
    /// <returns></returns>
    public Dictionary<BO.EngineerLevel, List<BO.Engineer>> GroupByEngineerLevel()
    {
        var engineersGrouped = _dal.Engineer.ReadAll()
            .GroupBy(engineer => (BO.EngineerLevel)engineer.EngineerLevel)
            .ToDictionary(
                group => group.Key,
                group => group.Select(doEngineer =>
                {
                    var taskForEngineer = GetEngineerTask(doEngineer.IdNum);
                    return new BO.Engineer
                    {
                        Id = doEngineer.IdNum,
                        Name = doEngineer.Name,
                        Email = doEngineer.Email,
                        Cost = doEngineer.CostPerHour,
                        Level = (BO.EngineerLevel)doEngineer.EngineerLevel!,
                        Task = taskForEngineer
                    };
                }).ToList()
            );

        return engineersGrouped;
    }
   
    /// <summary>
    /// A function that deletes all engineers
    /// </summary>
    public void clear()
    {
        _dal.Engineer.clear();
    }

  
}




