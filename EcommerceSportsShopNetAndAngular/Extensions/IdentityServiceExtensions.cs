using Core.Entities.Identity;
using Infrastracture.Data.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<AppIdentityDbContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("IdentityConnection"));
            });

            services.AddIdentityCore<AppUser>(opt =>
            {
                // add identity options here
            })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddSignInManager<SignInManager<AppUser>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = bool.Parse(config["JsonWebTokenKeys:ValidateIssuerSigningKey"]),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JsonWebTokenKeys:IssuerSigningKey"])),
                    ValidateIssuer = bool.Parse(config["JsonWebTokenKeys:ValidateIssuer"]),
                    ValidAudience = config["JsonWebTokenKeys:ValidAudience"],
                    ValidIssuer = config["JsonWebTokenKeys:ValidIssuer"],
                    ValidateAudience = bool.Parse(config["JsonWebTokenKeys:ValidateAudience"]),
                    RequireExpirationTime = bool.Parse(config["JsonWebTokenKeys:RequireExpirationTime"]),
                    ValidateLifetime = bool.Parse(config["JsonWebTokenKeys:ValidateLifetime"])
                };
            });


            services.AddAuthorization();

            return services;
        }
    }
}