using Microsoft.AspNetCore.Identity;

namespace HPCTechSpring2025MovieApp.Services;

public class RoleInitializerService
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleInitializerService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task InitializeRolesAsync()
    {
        string[] roleNames = { "Admin", "User" };
        foreach (var roleName in roleNames)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
