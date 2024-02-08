
using BO;

namespace BlApi;

public interface ITask
{
    public IEnumerable<BO.Task> ReadAll();// אפשר על ידי סינון 
    //IEnumerable<T> ReadAll(Func<T, bool>? filter = null);
    public void Update(BO.Task boTask);
    public int Creat(BO.Task boTask);
    public void Delete(int id);
    public BO.Task Read(int id);
    public void UpdateStartDate(int id, DateTime date);
    public void ProjectStartDateUpdate(DateTime date);
    public void ProjectEndDateUpdate(DateTime date);
   
}
