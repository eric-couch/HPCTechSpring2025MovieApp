using HPCTechSpring2025MovieApp.Data;
using Microsoft.AspNetCore.Identity;

namespace HPCTechSpring2025MovieApp.Services;

public class UserRoleService : IUserRolesService
{
    public readonly UserManager<IdentityUser> _userManager;
    public readonly RoleManager<IdentityRole> _roleManager;

    public async Task<IList<string>?> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new ArgumentException("User not found", nameof(userId));
        }

        return await _userManager.GetRolesAsync(user);
    }

    public async Task<bool> IsUserInRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new ArgumentException("User not found", nameof(userId));
        }
        return await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AddUserToRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new ArgumentException("User not found", nameof(userId));
        }
        if (!await _roleManager.RoleExistsAsync(role))
        {
            throw new ArgumentException("Role not found", nameof(role));
        }

        if (!await _userManager.IsInRoleAsync(user, role))
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }

        return true;
    }
    public async Task<bool> RemoveUserFromRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new ArgumentException("User not found", nameof(userId));
        }
        if (!await _roleManager.RoleExistsAsync(role))
        {
            throw new ArgumentException("Role not found", nameof(role));
        }
        if (await _userManager.IsInRoleAsync(user, role))
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role);
            return result.Succeeded;
        }
        return true;
    }
}
