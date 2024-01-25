
namespace BlApi;

public interface ITask
{
    public IEnumerable<BO.Task> ReadAll();// אפשר על ידי סינון 
    public void Update(int id);
    public int Creat(BO.Task item);
    public void Delete(int id);
    public BO.Task Read(int id);
    public void Update(int id, DateTime date);
}
