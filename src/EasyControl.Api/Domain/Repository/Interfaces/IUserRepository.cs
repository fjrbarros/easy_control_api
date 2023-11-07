using EasyControl.Api.Domain.Models;

namespace EasyControl.Api.Domain.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User?> GetByEmail(string email);
    }
}
