using BankAccounts.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Data
{
    public class LoginContext : DbContext
    {
        public LoginContext(DbContextOptions options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Transactions> transactions {get;set;}
    }
}
