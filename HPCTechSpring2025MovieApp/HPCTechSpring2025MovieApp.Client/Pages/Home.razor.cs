using HPCTechSpring2025MovieApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace HPCTechSpring2025MovieApp.Client.Pages;

public partial class Home
{
    [Inject]
    public HttpClient Http { get; set; }
    [Inject]
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
                this.user = await Http.GetFromJsonAsync<UserDto>("api/User");
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
