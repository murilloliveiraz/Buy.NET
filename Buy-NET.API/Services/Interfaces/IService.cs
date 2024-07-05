namespace Buy_NET.API.Services.Interfaces;

public interface IService<RQ, RS, I> where RQ : class
{
    Task<IEnumerable<RS>> Get ();
    Task<RS> GetById (I id);
    Task<RS> Create (RQ model);
    Task<RS> Update (I id, RQ model);
    Task Delete (I id);
}