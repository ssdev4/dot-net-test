using System;
namespace DotNetAPI.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IUserRepository _userRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

