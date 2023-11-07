using EasyControl.Api.Data;
using EasyControl.Api.Domain.Models;
using EasyControl.Api.Domain.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EasyControl.Api.Domain.Repository.Classes
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<User> Create(User user)
        {
            await _context.User.AddAsync(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<int> Delete(Guid id)
        {
            User? userEntity = await GetById(id);

            if (userEntity is null)
                return 0;

            _context.Entry(userEntity).State = EntityState.Deleted;

            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.User.AsNoTracking().ToListAsync();
        }

        public async Task<User?> GetById(Guid id)
        {
            return await _context.User.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _context.User.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> Update(User user)
        {
            User? userEntity = await GetById(user.Id);

            if (userEntity is null)
                return null;

            _context.Entry(userEntity).CurrentValues.SetValues(user);

            _context.Update(userEntity);

            await _context.SaveChangesAsync();

            return userEntity;
        }
    }
}
