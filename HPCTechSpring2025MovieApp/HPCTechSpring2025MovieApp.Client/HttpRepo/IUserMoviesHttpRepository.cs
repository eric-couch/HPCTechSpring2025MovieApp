using HPCTechSpring2025MovieApp.Shared;
using HPCTechSpring2025MovieApp.Shared.Wrapper;

namespace HPCTechSpring2025MovieApp.Client.HttpRepo;

public interface IUserMoviesHttpRepository
{
    Task<DataResponse<UserDto>> GetUserAsync();
}
