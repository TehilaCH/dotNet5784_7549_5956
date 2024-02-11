using BO;
using DalApi;
using DO;

namespace BlApi;

public interface IEngineer
{
    /// <summary>
    /// Creates new Engineer
    /// </summary>
    /// <param name="boEngineer"></param>
    /// <returns></returns>
    public int Creat( BO.Engineer boEngineer); 
    /// <summary>
    /// Update Engineer
    /// </summary>
    /// <param name="boEngineer"></param>
    /// 
    public void Update(BO.Engineer boEngineer);
    /// <summary>
    /// Delete Engineer according to id
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id);

    /// <summary>
    /// reads with and without filtering, i.e. returns an object by ID numbe
    /// </summary>
    /// <param name="id"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public BO.Engineer Read(int id, Func<DO.Engineer, bool>? filter = null);

    /// <summary>
    /// Returns a filterable list
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<DO.Engineer, bool>? filter = null);
    /// <summary>
    /// A function that assigns engineers by name
    /// </summary>
    /// <returns></returns>
    public List<BO.Engineer> OrderEngineers();
    /// <summary>
    /// A function that makes groups according to an engineer's level
    /// </summary>
    /// <returns></returns>
    public Dictionary<BO.EngineerLevel, List<BO.Engineer>> GroupByEngineerLevel();
    /// <summary>
    /// Deletes all engineers
    /// </summary>
    public void clear();
    

}
