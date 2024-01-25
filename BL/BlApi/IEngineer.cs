using DO;

namespace BlApi;

public interface IEngineer
{
   
    public int insert( Engineer item); 

    public void Update(Engineer item); 

    /* Engineer Read(int id); 
     IEnumerable<T?> ReadAll(Func<T, bool>? filter = null);
     void Update(T item); 
     void Delete(int id); */

}
