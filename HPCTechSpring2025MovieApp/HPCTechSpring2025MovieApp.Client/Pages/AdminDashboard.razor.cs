using HPCTechSpring2025MovieApp.Shared;
using HPCTechSpring2025MovieApp.Shared.Wrapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace HPCTechSpring2025MovieApp.Client.Pages;

public partial class AdminDashboard
{
    [Inject]
    public HttpClient Http { get; set; }
    [Inject]
    public AuthenticationStateProvider asp { get; set; } = default!;
    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    public List<UserEditDto> Users { get; set; } = new List<UserEditDto>();
    public string ErrorMessage { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            // Use the NavigationManager to construct the absolute URI
            // This is more reliable than relying on HttpClient.BaseAddress
            string baseUri = NavigationManager.BaseUri;
            string requestUri = $"{baseUri.TrimEnd('/')}/api/GetUsers";

            Console.WriteLine($"Making request to: {requestUri}");

            // Make the request with the absolute URI string
            var res = await Http.GetFromJsonAsync<DataResponse<List<UserEditDto>>>(requestUri);
            if (res?.Succeeded == true)
            {
                Users = res.Data;
            }
            else
            {
                ErrorMessage = $"API call successful but returned failure status. Message: {res?.Message}";
            }
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = $"HTTP Request Error: {ex.Message}";
            Console.WriteLine($"HTTP Error: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
        }
        catch (InvalidOperationException ex)
        {
            ErrorMessage = $"Invalid Operation: {ex.Message}";
            Console.WriteLine($"Invalid Operation: {ex.Message}");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"General Error: {ex.Message}";
            Console.WriteLine($"General Exception: {ex.Message}");
            Console.WriteLine($"Exception Type: {ex.GetType().Name}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
        }
    }
}
