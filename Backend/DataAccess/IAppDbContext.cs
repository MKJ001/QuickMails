namespace DataAccess
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Models;

    public interface IAppDbContext : IDisposable
    {
        DbSet<EmailGroup> EmailGroups { get; set; }

        DbSet<Email> Emails { get; set; }

        DatabaseFacade Database { get; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
