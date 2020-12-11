using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class EFUsersContext : DbContext
    {
        public EFUsersContext(DbContextOptions<EFUsersContext> options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
    }
}
