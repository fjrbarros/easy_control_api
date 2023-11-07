namespace EasyControl.Api.Domain.Services.Interfaces
{
    /// <summary>
    /// Generic interface for creating CRUD of type services
    /// </summary>
    /// <typeparam name="RQ">Request contract</typeparam>
    /// <typeparam name="RS">Response contract</typeparam>
    /// <typeparam name="I">ID type</typeparam>
    public interface IService<RQ, RS, I>
        where RQ : class
    {
        Task<IEnumerable<RS>> GetAll(I userId);
        Task<RS> GetById(I id, I userId);
        Task<RS> Create(RQ entity, I userId);
        Task<RS> Update(RQ entity, I userId);
        Task Inactivate(I id, I userId);
    }
}
