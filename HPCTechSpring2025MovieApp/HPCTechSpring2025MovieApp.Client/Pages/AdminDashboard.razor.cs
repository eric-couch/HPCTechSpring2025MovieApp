using HPCTechSpring2025MovieApp.Shared;
using HPCTechSpring2025MovieApp.Shared.Wrapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json.Bson;
using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
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
    public UserEditDto SelectedUser { get; set; }
    public EditForm editForm { get; set; }
    //public UserEditDto userEditDto { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public bool IsUserModalVisible { get; set; } = false;
    public string requestUri { get; set; } = string.Empty;
    protected override async Task OnInitializedAsync()
    {
        try
        {
            // Use the NavigationManager to construct the absolute URI
            // This is more reliable than relying on HttpClient.BaseAddress
            string baseUri = NavigationManager.BaseUri;
            requestUri = $"{baseUri.TrimEnd('/')}/api/GetUsers";

            Console.WriteLine($"Making request to: {requestUri}");

            await LoadGrid(requestUri);
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

    public async Task LoadGrid(string requestUri)
    {
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

    public async Task ToggleEmailConfirmed(Microsoft.AspNetCore.Components.ChangeEventArgs args, string userId)
    {
        try
        {
            var res = await Http.GetFromJsonAsync<Response>($"api/ToggleEmailConfirmed?userId={userId}");

            if (!res.Succeeded)
            {
                ErrorMessage = $"Error updating Email Confirmed for user {userId}";
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

    public async Task AddUserOnSubmit()
    {
        // call api/UpdateUser
        var res = await Http.PostAsJsonAsync($"api/update-user", SelectedUser);

        if (res.IsSuccessStatusCode)
        {
            // make modal hidden
            IsUserModalVisible = false;
            // reload grid
            await LoadGrid(requestUri);
        }

    }

    public async Task ToggleAdmin(Microsoft.AspNetCore.Components.ChangeEventArgs args, string userId)
    {
        try
        {
            var res = await Http.GetFromJsonAsync<Response>($"api/ToggleAdmin?userId={userId}");

            if (!res.Succeeded)
            {
                ErrorMessage = $"Error updating Email Confirmed for user {userId}";
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

    public async Task EditUser()
    {
        // open modal dialog box with user selected as edit record
        IsUserModalVisible = true;
    }

    public void Reset()
    {
        SelectedUser = new UserEditDto();
        IsUserModalVisible = false;
    }

    public async Task GetUserRecordSelected(RowSelectEventArgs<UserEditDto> args)
    {
        SelectedUser = args.Data;
    }

    public async Task ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
    {
        if (args.Item.Id == "GridUserAdd")
        {
            IsUserModalVisible = true;
            //await AddUser();
        }
    }
}
