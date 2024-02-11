using BO;
using DalApi;
using DO;

namespace BlApi;

public interface IEngineer
{
    public int Creat( BO.Engineer boEngineer); //Creates new Engineer
    public void Update(BO.Engineer boEngineer);//Update Engineer
    public void Delete(int id);//Delete Engineer
    public BO.Engineer Read(int id, Func<DO.Engineer, bool>? filter = null);//
    public IEnumerable<BO.Engineer> ReadAll(Func<DO.Engineer, bool>? filter = null);
    public List<BO.Engineer> OrderEngineers();
    public Dictionary<BO.EngineerLevel, List<BO.Engineer>> GroupByEngineerLevel();
    public void clear();
    

}
