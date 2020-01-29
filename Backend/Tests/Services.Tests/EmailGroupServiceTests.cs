namespace Services.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess.Models;
    using FluentAssertions;
    using Models;
    using Moq;
    using NUnit.Framework;

    public class EmailGroupServiceTests : TestBase
    {
        private EmailGroupService service;

        private EmailGroup group1;
        private EmailGroup group2;

        protected override void InitializeTest()
        {
            this.group1 = new EmailGroup()
            {
                Id = 1,
                Name = "Group1",
                Emails = new List<Email>()
                {
                    new Email()
                    {
                        Id = 1,
                        Address = "email1@example.com"
                    },
                    new Email()
                    {
                        Id = 2,
                        Address = "email2@example.com"
                    },
                }
            };

            this.group2 = new EmailGroup()
            {
                Id = 2,
                Name = "Group2",
                Emails = new List<Email>()
                {
                    new Email()
                    {
                        Id = 3,
                        Address = "other1@example.com"
                    },
                    new Email()
                    {
                        Id = 4,
                        Address = "other2@example.com"
                    },
                }
            };

            this.DbContext.EmailGroups.AddRange(this.group1, this.group2);
            this.DbContext.SaveChanges();

            var validatorServiceMock = new Mock<IEmailGroupsDbValidatorService>();
            validatorServiceMock
                .Setup(m => m.ThrowIfGroupNameAlreadyExist(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            this.service = new EmailGroupService(this.DbContext, validatorServiceMock.Object);
        }

        [Test]
        public async Task GetAll_ShouldReturnCorrectData()
        {
            var result = await this.service.GetAll();

            result.Should().ContainEquivalentOf(new EmailGroupDto
            {
                Id = this.group1.Id,
                Name = this.group1.Name,
                Emails = this.group1.Emails.Select(item => item.Address)
            });

            result.Should().ContainEquivalentOf(new EmailGroupDto 
            {
                Id = this.group2.Id,
                Name = this.group2.Name,
                Emails = this.group2.Emails.Select(item => item.Address)
            });
        }

        [Test]
        public async Task Create_ValidData_ReturnsCorrectData()
        {
            var result = await this.service.Create(new NewEmailGroupDto() {
                Name = "TestCase1",
                Emails = new [] { "a1@b.com", "a2@b.com" }
            });

            result.Should().BeEquivalentTo(new 
            {
                Name = "TestCase1",
                Emails = new[] { "a1@b.com", "a2@b.com" }
            });
        }


        [Test]
        public async Task Create_ValidData_CreatesCorrectDatabaseEntries()
        {
            var emails = new[] {"b1@a.com", "b2@a.com"};
            var dto = new NewEmailGroupDto()
            {
                Name = "TestCase2",
                Emails = emails
            };

            var created = await this.service.Create(dto);

            this.DbContext.EmailGroups.Any(item => item.Name == dto.Name).Should().BeTrue();
            this.DbContext.Emails.Any(item => item.Address == emails[0] && item.EmailGroupId == created.Id).Should().BeTrue();
            this.DbContext.Emails.Any(item => item.Address == emails[1] && item.EmailGroupId == created.Id).Should().BeTrue();
        }
    }
}
