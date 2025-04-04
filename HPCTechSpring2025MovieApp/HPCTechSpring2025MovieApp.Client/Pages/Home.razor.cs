using HPCTechSpring2025MovieApp.Client.HttpRepo;
using HPCTechSpring2025MovieApp.Shared;
using HPCTechSpring2025MovieApp.Shared.Wrapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace HPCTechSpring2025MovieApp.Client.Pages;

public partial class Home
{
    [Inject]
    public IUserMoviesHttpRepository UserMoviesHttpRepository { get; set; }
    public AuthenticationStateProvider asp { get; set; }
    public UserDto user { get; set; } = new UserDto();
    bool isAuthenticated = false;
    protected override async Task OnInitializedAsync()
    {
        var authState = await asp.GetAuthenticationStateAsync();

        if (authState.User?.Identity?.IsAuthenticated == true)
        {
            isAuthenticated = true;
            try
            {
                //this.user = await Http.GetFromJsonAsync<UserDto>("api/User");
                DataResponse<UserDto> userDto = await UserMoviesHttpRepository.GetUserAsync();
                if (userDto.Succeeded)
                {
                    user = userDto.Data;
                } else
                {
                    // add toast using the userDto.Message and userDto.Errors
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // TODO: add logging
            }

        } else
        {
            
        }
    }
}
