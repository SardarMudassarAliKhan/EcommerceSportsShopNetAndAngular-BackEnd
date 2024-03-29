using API.Extensions;
using Core.Entities.Identity;
using Core.Interfaces;
using EcommerceSportsShopNetAndAngular.Errors;
using EcommerceSportsShopNetAndAngular.Extensions;
using EcommerceSportsShopNetAndAngular.Middlewere;
using Infrastracture.Data; // Make sure to import the namespace for AppDbContext
using Infrastracture.Data.Identity;
using Infrastracture.Repositories;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Make sure to import the namespace for Entity Framework Core

namespace EcommerceSportsShopNetAndAngular
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Adding Services to the IOC container
            builder.Services.AddControllers();
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddSwaggerDocumentation();
            
            var app = builder.Build();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.UseCors("CorsPolicy");
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<AppDbContext>();
            var identityContext = services.GetRequiredService<AppIdentityDbContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var logger = services.GetRequiredService<ILogger<Program>>();
            try
            {
                 context.Database.MigrateAsync();
                 identityContext.Database.MigrateAsync();
                 AppIdentityDbContextSeed.SeedUsersAsync(userManager);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured during migration");
            }


            app.Run();
        }
    }
}
