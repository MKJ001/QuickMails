namespace Services.Tests
{
    using DataAccess;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System;

    public abstract class TestBase
    {
        protected AppDbContext DbContext;

        protected abstract void InitializeTest();
        
        [OneTimeSetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            this.DbContext = new AppDbContext(options);

            this.InitializeTest();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            this.DbContext.Dispose();
        }
    }
}