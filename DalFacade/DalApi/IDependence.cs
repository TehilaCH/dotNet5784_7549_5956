
namespace DalApi;
using DO;
public interface IDependence
{
    int Create(IDependence item); //Creates new entity object in DAL
    IDependence? Read(int id); //Reads entity object by its ID 
    List<IDependence> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(IDependence item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id

}
