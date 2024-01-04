
namespace DalApi;
using DO;
public interface ITask
{
    int Create(ITask item); //Creates new entity object in DAL
    ITask? Read(int id); //Reads entity object by its ID 
    List<ITask> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(ITask item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id

}
