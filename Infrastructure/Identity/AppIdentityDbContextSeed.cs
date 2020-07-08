using System.Linq;
using System.Threading.Tasks;
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
                var user = new AppUser {
                    DisplayName="Ramesh Chaudhary",
                    UserName="Ramesh",
                    Email="ramesh@dadlani.com",
                    Address = new Address{
                        FirstName = "Ramesh",
                        LastName="Chaudahry",
                        Street = "25, BDD Blocks",
                        Zipcode = "400102",
                        State = "Maharashtra"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}