namespace Buy_NET.API.Services.Interfaces;

public interface IService<RQ, RS, I> where RQ : class
{
    Task<IEnumerable<RS>> Get (I idUser);
    Task<RS> GetById (I idUser, I id);
    Task<RS> Create (RQ model, I idUser);
    Task<RS> Update (I id, RQ model, I idUser);
    Task Delete (I id, I idUser);
}