using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "smak",
                    Email = "smak@test.com",
                    UserName = "bob@test.com",
                    Address = new Address
                    {
                        FirstName = "Sardar Mudassar",
                        LastName = "Ali Khan",
                        Street = "10 The street",
                        City = "Abbottabad",
                        State = "KPK",
                        Zipcode = "22010"
                    }
                };

                await userManager.CreateAsync(user, "Test@1234");
            }
        }
    }
}