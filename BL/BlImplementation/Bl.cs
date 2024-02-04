
namespace BlImplementation;
using BlApi;
internal class Bl : IBl
{
    public IEngineer Engineer =>  new EngineerImplementation();

    public ITask Task => new TaskImplementation();


    //public DateTime? StartProjectDate { get => throw new NotImplementedException();}
    //public DateTime? EndProjectDate { get => throw new NotImplementedException();}


}
