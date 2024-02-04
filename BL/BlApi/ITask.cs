
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

    //**********
    public Status stat(DO.Task boTask);//לבדוק אם לכתוב כאן
    public DateTime? CalculationOfDeadline(DO.Task doTask);
    public EngineerInTask? EngineerToTask(DO.Task doTask);
    public List<TaskInList>? ReDependent(DO.Task doTask);
    public ProjectStatus projectlevel(DO.Task doTask);
}
