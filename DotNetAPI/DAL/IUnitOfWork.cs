using System;
namespace DotNetAPI.DAL
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
