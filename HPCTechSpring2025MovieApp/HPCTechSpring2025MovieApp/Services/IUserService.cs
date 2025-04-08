using HPCTechSpring2025MovieApp.Data;

namespace HPCTechSpring2025MovieApp.Services;

public interface IUserService
{
    Task<ApplicationUser> GetUserByUserNameAsync(string userName);
}
