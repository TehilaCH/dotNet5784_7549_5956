
using BO;
using DalApi;

namespace BlApi;

public interface ITask
{
    public IEnumerable<BO.Task> ReadAll(Func<DO.Task, bool>? filter = null);
    public void Update(BO.Task boTask);
    public int Creat(BO.Task boTask);
    public void Delete(int id);
    public BO.Task Read(int id, Func<DO.Task, bool>? filter = null);
    public void UpdateStartDate(int id, DateTime date);
    public void clear();
  
}
