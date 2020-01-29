namespace Services
{
    using Common.Exceptions;
    using DataAccess;
    using DataAccess.Models;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IEmailGroupService
    {
        Task<EmailGroupDto> Create(NewEmailGroupDto dto);

        Task<EmailGroupDto> Update(int groupId, NewEmailGroupDto dto);

        Task Delete(int groupId);

        Task<IEnumerable<EmailGroupDto>> GetAll();
    }

    public class EmailGroupService : IEmailGroupService
    {
        private readonly IAppDbContext dbContext;
        private readonly IEmailGroupsDbValidatorService validatorService;

        public EmailGroupService(IAppDbContext dbContext, IEmailGroupsDbValidatorService validatorService)
        {
            this.dbContext = dbContext;
            this.validatorService = validatorService;
        }

        public async Task<EmailGroupDto> Create(NewEmailGroupDto dto)
        {
            await this.validatorService.ThrowIfGroupNameAlreadyExist(dto.Name);

            var newGroup = new EmailGroup()
            {
                Name = dto.Name,
                Emails = dto.Emails.Select(item => new Email()
                {
                    Address = item
                }).ToList()
            };

            this.dbContext.EmailGroups.Add(newGroup);

            await this.dbContext.SaveChangesAsync();

            return new EmailGroupDto()
            {
                Id = newGroup.Id,
                Name = newGroup.Name,
                Emails = newGroup.Emails.Select(item => item.Address)
            };
        }
        
        public async Task<EmailGroupDto> Update(int groupId, NewEmailGroupDto dto)
        {
            var emailGroup = await this.GetEmailGroupSafely(groupId);

            this.dbContext.Emails.RemoveRange(emailGroup.Emails);

            emailGroup.Name = dto.Name;
            emailGroup.Emails = dto.Emails.Select(item => new Email()
            {
                Address = item
            }).ToList();

            await this.dbContext.SaveChangesAsync();

            return new EmailGroupDto()
            {
                Id = emailGroup.Id,
                Name = emailGroup.Name,
                Emails = emailGroup.Emails.Select(item => item.Address)
            };
        }
        
        public async Task Delete(int groupId)
        {
            var emailGroup = await this.GetEmailGroupSafely(groupId);

            this.dbContext.Emails.RemoveRange(emailGroup.Emails);
            this.dbContext.EmailGroups.Remove(emailGroup);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<EmailGroupDto>> GetAll()
        {
            var groups = await this.dbContext.EmailGroups
                .Include(item => item.Emails)
                .ToListAsync();

            return groups.Select(item => new EmailGroupDto()
            {
                Id = item.Id,
                Name = item.Name,
                Emails = item.Emails.Select(e => e.Address)
            });
        }

        private async Task<EmailGroup> GetEmailGroupSafely(int groupId)
        {
            var emailGroup = await this.dbContext.EmailGroups
                .Include(item => item.Emails)
                .FirstOrDefaultAsync(item => item.Id == groupId);

            if (emailGroup is null)
            {
                throw new AppNotFoundException($"Group ID: {groupId} doesn't exists.");
            }

            return emailGroup;
        }
    }
}
