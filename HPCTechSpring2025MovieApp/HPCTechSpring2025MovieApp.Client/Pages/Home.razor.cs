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
    public HttpClient Http { get; set; }
    //[Inject]
    //public IUserMoviesHttpRepository UserMoviesHttpRepository { get; set; } = default!;
    [Inject]
    public AuthenticationStateProvider asp { get; set; } = default!;
    public UserDto userDto { get; set; } = new UserDto();
    bool isAuthenticated = false;
    protected override async Task OnInitializedAsync()
    {
        var authState = await asp.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            isAuthenticated = true;
            try
            {
                DataResponse<UserDto> res = await Http.GetFromJsonAsync<DataResponse<UserDto>>("api/User");
                //DataResponse<UserDto> res = await UserMoviesHttpRepository.GetUserAsync();
                if (res.Succeeded)
                {
                    userDto = res.Data;
                }
                else
                {
                    // add toast using the userDto.Message and userDto.Errors
                }


                
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // TODO: add logging
            }

        }
        else
        {

        }
    }
}
