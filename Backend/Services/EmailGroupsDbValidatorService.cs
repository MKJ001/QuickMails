namespace Services
{
    using Common.Exceptions;
    using DataAccess;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;

    public interface IEmailGroupsDbValidatorService
    {
        Task ThrowIfGroupNameAlreadyExist(string groupName);
    }
    
    public class EmailGroupsDbValidatorService : IEmailGroupsDbValidatorService
    {
        private readonly IAppDbContext dbContext;

        public EmailGroupsDbValidatorService(IAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task ThrowIfGroupNameAlreadyExist(string groupName)
        {
            var exists = await this.dbContext.EmailGroups
                .AnyAsync(item => string.Equals(item.Name, groupName, StringComparison.InvariantCultureIgnoreCase));
            
            if (exists)
            {
                var message = $"Group with name {groupName} already exists.";
                throw new AppBadRequestException(message, AppExceptionCodes.GroupNameAlreadyExists);
            }
        }
    }
}
