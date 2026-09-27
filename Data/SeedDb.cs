using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraHub.Data.Entities;
using LibraHub.Helpers;

namespace LibraHub.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Reader");

            var user = await _userHelper.GetUserByEmailAsync("jovanamatos22@gmail.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Jovana",
                    LastName = "Matos",
                    UserName = "jovanamatos22@gmail.com",
                    Email = "jovanamatos22@gmail.com"
                };

                var result = await _userHelper.AddUserAsync(
                    user,
                    "123456"
                );
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        throw new InvalidOperationException(
                            $"{error.Code}: {error.Description}"
                        );
                    }
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");

            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }
        }
    }
}
