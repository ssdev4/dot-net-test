using System;
using DotNetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetAPI.DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync(string lastName = null, bool ascending = true)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(lastName))
            {
                query = query.Where(u => u.LastName == lastName);
            }

            if (ascending)
            {
                query = query.OrderBy(u => u.LastName);
            }
            else
            {
                query = query.OrderByDescending(u => u.LastName);
            }

            return await query.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}

