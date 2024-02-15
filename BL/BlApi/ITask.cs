
using BO;
using DalApi;

namespace BlApi;

public interface ITask
{
    /// <summary>
    /// Returns a filterable list of tasks
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null);
    /// <summary>
    /// Updates an existing task
    /// </summary>
    /// <param name="boTask"></param>
    public void Update(BO.Task boTask);
    /// <summary>
    /// Creates a task only in the project planning phase
    /// </summary>
    /// <param name="boTask"></param>
    /// <returns></returns>
    public int Creat(BO.Task boTask);
    /// <summary>
    /// Deletes a task that has no dependent tasks
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id);
    /// <summary>
    /// Returns a filterable object
    /// </summary>
    /// <param name="id"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public BO.Task Read(int id);
    /// <summary>
    /// Updates a scheduled task start date
    /// </summary>
    /// <param name="id"></param>
    /// <param name="date"></param>
    public void UpdateStartDate(int id, DateTime date);
    /// <summary>
    /// Deletes all tasks and dependencies
    /// </summary>
    public void clear();
  
}
