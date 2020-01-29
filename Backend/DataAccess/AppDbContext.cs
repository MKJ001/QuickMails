namespace DataAccess
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class AppDbContext : DbContext, IAppDbContext
    {
        public DbSet<EmailGroup> EmailGroups { get; set; }

        public DbSet<Email> Emails { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
