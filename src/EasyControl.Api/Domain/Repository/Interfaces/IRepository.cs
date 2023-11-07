namespace EasyControl.Api.Domain.Repository.Interfaces
{
    public interface IRepository<T, I>
        where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetById(I id);
        Task<T> Create(T entity);
        Task<T?> Update(T entity);
        Task<int> Delete(I id);
    }
}
