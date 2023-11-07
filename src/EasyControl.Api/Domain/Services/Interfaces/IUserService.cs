using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyControl.Api.Contract.User;

namespace EasyControl.Api.Domain.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserCreateResponseContract>> GetAll();
        Task<UserCreateResponseContract> GetById(Guid userId);
        Task<UserCreateResponseContract> Create(UserCreateRequestContract entity);
        Task<UserCreateResponseContract> Update(Guid userId, UserCreateRequestContract entity);
        Task Delete(Guid userId);
        Task<UserCreateResponseContract> GetByEmail(string email);
        Task<UserLoginResponseContract> Authentication(UserLoginRequestContract entity);
    }
}
