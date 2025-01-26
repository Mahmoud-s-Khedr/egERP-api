using System;
using EG_ERP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EG_ERP.Utils;

public static class Extensions
{
    public static void SeedRoles(this ModelBuilder modelBuilder)
    {
        string[] coreRoles = { "Admin", "Manager", "Employee" };
        // string[] departmentRoles = { "HR", "IT", "Finance", "Sales" };

        List<AppRole> appRoles = coreRoles
            // .Concat(departmentRoles)
            .Distinct()
            .Select((roleName, idx) => new AppRole
            {
                Id = idx + 1,
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            })
            .ToList();

        modelBuilder.Entity<AppRole>().HasData(appRoles);
    }

    public static void SeedAdmin(this ModelBuilder modelBuilder, IConfiguration config)
    {
        List<User> users = config.GetSection("Admins").Get<List<User>>() ?? new List<User>();
        PasswordHasher<AppUser> hasher = new PasswordHasher<AppUser>();

        List<Admin> admins = users.Select((user, idx) => new Admin
        {
            Id = idx + 1,
            Name = user.Name ?? "",
            UserName = user.UserName,
            NormalizedUserName = user.UserName?.ToUpper(),
            Email = user.Email,
            NormalizedEmail = user.Email?.ToUpper(),
            EmailConfirmed = true,
            Vertified = true,
            PasswordHash = hasher.HashPassword(new Admin(), user.Password ?? throw new Exception("Password is required"))
        }).ToList();

        modelBuilder.Entity<Admin>().HasData(admins);
        
        int adminRoleId = 1;
        List<IdentityUserRole<int>> userRoles = admins.Select(admin => new IdentityUserRole<int>
        {
            UserId = admin.Id,
            RoleId = adminRoleId
        }).ToList();
        modelBuilder.Entity<IdentityUserRole<int>>().HasData(userRoles);
    }

    internal class User
    {
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
