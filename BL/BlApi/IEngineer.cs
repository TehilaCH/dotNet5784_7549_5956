using DO;

namespace BlApi;

public interface IEngineer
{
   
    public int Creat( BO.Engineer boEngineer); 
    public void Update(BO.Engineer boEngineer);
    public void Delete(int id);
    public Engineer Read(int id);
    public IEnumerable<BO.Engineer> ReadAll();


    
    // IEnumerable<T?> ReadAll(Func<T, bool>? filter = null); אפשר גם לעשות לפי פונקציה שתסנן ךדוגמא לפי רמת מהנדס
    
}
