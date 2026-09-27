using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LibraHub.Data.Entities;

namespace LibraHub.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedDb(
            DataContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            }

            if (!await _roleManager.RoleExistsAsync("Reader"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Reader" });
            }

            var user = await _userManager.FindByEmailAsync("jovanamatos22@gmail.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Jovana",
                    LastName = "Matos",
                    UserName = "jovanamatos22@gmail.com",
                    Email = "jovanamatos22@gmail.com"
                };

                var result = await _userManager.CreateAsync(user, "123456");

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        throw new InvalidOperationException(
                            $"{error.Code}: {error.Description}"
                        );
                    }
                }

                await _userManager.AddToRoleAsync(user, "Admin");
            }

            var isInRole = await _userManager.IsInRoleAsync(user, "Admin");

            if (!isInRole)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
