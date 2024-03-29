using System.Threading.Tasks;
using ContactBook.Model.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.Data
{
    public class ContactDbContext : IdentityDbContext
    {
        public ContactDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; }
    }
}