namespace Services.Tests
{
    using Common.Exceptions;
    using DataAccess.Models;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    public class EmailGroupsDbValidatorServiceTests : TestBase
    {
        private EmailGroup group1;
        private EmailGroupsDbValidatorService service;

        protected override void InitializeTest()
        {
            this.group1 = new EmailGroup()
            {
                Id = 1,
                Name = "SomeGroupName"
            };
            this.DbContext.EmailGroups.Add(this.group1);

            this.DbContext.SaveChanges();

            this.service = new EmailGroupsDbValidatorService(this.DbContext);
        }

        [Test]
        [TestCase("SomeGroupName")]
        [TestCase("SoMeGrOupNAme")]
        [TestCase("somegroupname")]
        public async Task GroupAlreadyExists_ShouldNotThrowException(string groupName)
        {
            Func<Task> action = async () => await this.service.ThrowIfGroupNameAlreadyExist(groupName);

            await action.Should().ThrowExactlyAsync<AppBadRequestException>();
        }

        [Test]
        public async Task InvalidGroupName_ShouldThrowException()
        {
            Func<Task> action = async () => await this.service.ThrowIfGroupNameAlreadyExist("SomeNewName");

            await action.Should().NotThrowAsync();
        }
    }
}
