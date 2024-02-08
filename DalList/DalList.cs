
namespace Dal;
using DalApi;
using DO;

sealed internal class DalList : IDal
{
    public static IDal Instance { get;} = new DalList();
    private DalList() { }
    public IEngineer Engineer => new EngineerImplementation();

    public IDependence Dependence =>  new DependenceImplementation();

    public ITask Task =>  new TaskImplementation();

    /*********/
    public DateTime? StartProjectDate { get => Instance.StartProjectDate; set => Instance.StartProjectDate = value; }
    public DateTime? EndProjectDate { get => Instance.EndProjectDate; set => Instance.EndProjectDate = value; }
    

}
