namespace BlImplementation;
using BlApi;
using BO;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = Factory.Get;

    public int Creat(BO.Engineer boEngineer)
    {
        if (boEngineer.Id < 1 || boEngineer.Cost < 0 || boEngineer.Name == null || boEngineer.Email == null)
        {
            throw new NotImplementedException("One or more parameters are incorrect");
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
            throw new NotImplementedException($"Student with ID={boEngineer.Id} already exists", ex);
        }
        
    }

    public void Delete(int id)//לעשות את הבדיקה השניה כאשר המהנדס באמצע משימה לפי הסטטוס
    {
        var taskForEngineer = _dal.Task.ReadAll()
           .FirstOrDefault(task => task.EngineerIdToTask == id);
        if(taskForEngineer == null)
        {
            throw new NotImplementedException();
        }
        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (NotImplementedException ex)
        {
            Console.WriteLine(ex);
        }
       
    }

    public DO.Engineer Read(int id)
    {

        //DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        //if (doEngineer == null)
        //    throw new NotImplementedException($"Engineer with ID={id} does Not exist");

        //var taskForEngineer = _dal.Task.ReadAll()
        //    .FirstOrDefault(task => task.EngineerIdToTask == doEngineer.IdNum);
        //return new BO.Engineer
        //{
        //    Id = id,
        //    Name = doEngineer.Name,
        //    Email = doEngineer.Email,
        //    Cost = doEngineer.CostPerHour,
        //    Level = (BO.EngineerLevel?)doEngineer.EngineerLevel,
        //    Task = taskForEngineer != null ? new BO.TaskInEngineer
        //    {
        //        Id = taskForEngineer.TaskId,
        //        NickName = taskForEngineer.Nickname
        //    } : null
        //};

        throw new NotImplementedException($"Engineer with ID={id} does Not exist");

    }

    public IEnumerable<BO.Engineer> ReadAll()
    {
        return(from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
               let taskForEngineer = _dal.Task.ReadAll().FirstOrDefault(task => task.EngineerIdToTask == doEngineer.IdNum)
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
        if (boEngineer.Id < 1 || boEngineer.Cost < 0 || boEngineer.Name == null || boEngineer.Email == null)
        {
            throw new NotImplementedException("One or more parameters are incorrect");
        }
        DO.Engineer doEngineer = new DO.Engineer(
           boEngineer.Id,
           boEngineer.Name,
           boEngineer.Email,
           (DO.EngineerLevel?)boEngineer.Level,
           boEngineer.Cost);
        DO.Engineer currentEngineer = _dal.Engineer.Read(boEngineer.Id);

        //if (currentEngineer != null)
        //{
        //    // Check if the level is not decreasing
        //    if (boEngineer.Level < currentEngineer.EngineerLevel)
        //    {
        //        throw new InvalidOperationException("Engineer level cannot be decreased.");
        //    }
        //}

      
        try 
        {
            _dal.Engineer.Update(doEngineer);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }

    }
}




