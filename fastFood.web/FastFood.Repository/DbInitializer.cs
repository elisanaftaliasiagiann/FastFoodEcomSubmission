﻿using FastFood.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Repository
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }

                if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole("Customer")).GetAwaiter().GetResult();

                    var user = new ApplicationUser
                    {
                        UserName = "admin@fastfood.com",
                        Email = "admin@fastfood.com",
                        Name = "Admin User"
                    };

                    _userManager.CreateAsync(user, "Admin123*").GetAwaiter().GetResult();
                    _userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
                }
            }
            catch (Exception)
            {
                // Handle error
            }
        }
    }
}
