namespace BlImplementation;
using BlApi;
internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = Factory.Get;

    public int Creat(BO.Engineer item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public DO.Engineer Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.Engineer> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Engineer item)
    {
        throw new NotImplementedException();
    }
}
