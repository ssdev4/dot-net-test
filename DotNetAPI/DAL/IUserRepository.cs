using System;
using DotNetAPI.Models;

namespace DotNetAPI.DAL
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync(string lastName = null, bool ascending = true);
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}

