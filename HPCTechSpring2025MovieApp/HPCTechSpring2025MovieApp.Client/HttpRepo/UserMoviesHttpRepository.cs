using HPCTechSpring2025MovieApp.Shared;
using HPCTechSpring2025MovieApp.Shared.Wrapper;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace HPCTechSpring2025MovieApp.Client.HttpRepo;

public class UserMoviesHttpRepository : IUserMoviesHttpRepository
{
    public readonly HttpClient _httpClient;

    public UserMoviesHttpRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DataResponse<UserDto>> GetUserAsync()
    {
        DataResponse<UserDto> userDto = await _httpClient.GetFromJsonAsync<DataResponse<UserDto>>("api/User");

        if (userDto.Succeeded)
        {
            return userDto;
        } else
        {
            return new DataResponse<UserDto>
            {
                Succeeded = false,
                Message = userDto.Message,
                Errors = userDto.Errors
            };
        }
    }
}
