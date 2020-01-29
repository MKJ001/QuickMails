namespace QuickMails
{
    using System;
    using DataAccess;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Middleware;
    using Services;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var envConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            var connectionString = string.IsNullOrEmpty(envConnectionString) ? this.configuration["Database:ConnectionString"] : envConnectionString;
            
            services.AddDbContext<IAppDbContext, AppDbContext>(options => options.UseMySql(connectionString));

            services.AddTransient<IEmailGroupsDbValidatorService, EmailGroupsDbValidatorService>();
            services.AddTransient<IEmailGroupService, EmailGroupService>();

            services.AddMvc(options => options.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAppDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            dbContext.Database.Migrate();

            app.UseMiddleware<AppExceptionHandlerMiddleware>();

            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyHeader();
                options.AllowAnyMethod();
            });

            app.UseMvc();
        }
    }
}
