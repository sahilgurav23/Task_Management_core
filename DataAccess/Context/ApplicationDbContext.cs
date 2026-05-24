using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class ApplicationDbContext: DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Profile> Profiles { get; set; }
    }
}
