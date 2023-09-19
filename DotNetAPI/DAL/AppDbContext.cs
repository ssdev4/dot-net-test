using System;
using DotNetAPI.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DotNetAPI.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
