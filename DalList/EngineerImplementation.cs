
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Xml.Linq;

public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {

        foreach (var engineer in DataSource.Engineers)
        {
            if (engineer.IdNum == item.IdNum)
            {
                throw new InvalidOperationException("Engineer with the same IdNum already exists");
            }
        }
        DataSource.Engineers.Add(item);

        throw new NotImplementedException();//בדיקה למה לא עובד כשמוחקים את השורה
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Engineer? Read(int id) 
    {
        foreach (var engineer in DataSource .Engineers)
        {
            if(engineer.IdNum==id)
            {
                return engineer;    
            }
        }
       return null; 
    }

    public List<Engineer> ReadAll()
    {
      //=new List<Engineer>(DataSource.Engineers);
        throw new NotImplementedException();
    }

    public void Update(Engineer item)
    {
        foreach(var engineer in DataSource .Engineers)
        {
            if(engineer.IdNum==item.IdNum)
            {
               
                DataSource.Engineers.Add(item);
            }
        }
      
        
        throw new NotImplementedException();
        
    }
}
