using Microsoft.EntityFrameworkCore;
using PicheTestTask.Domain.Models;

namespace PicheTestTask.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        { }

        public DbSet<Account> Accounts { get; set; }
    }
}
