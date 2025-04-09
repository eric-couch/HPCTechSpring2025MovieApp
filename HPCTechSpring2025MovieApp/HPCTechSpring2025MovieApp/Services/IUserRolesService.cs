namespace HPCTechSpring2025MovieApp.Services;

public interface IUserRolesService
{
    Task<IList<string>?> GetUserRolesAsync(string userId);
    Task<bool> IsUserInRoleAsync(string userId, string role);
    Task<bool> AddUserToRoleAsync(string userId, string role);
    Task<bool> RemoveUserFromRoleAsync(string userId, string role);
}
