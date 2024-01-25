namespace BlImplementation;
using BlApi;
using BO;
internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = Factory.Get;

    public int Creat(Task item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Task> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(int id, DateTime date)
    {
        throw new NotImplementedException();
    }
}
