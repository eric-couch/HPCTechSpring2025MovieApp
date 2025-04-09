using HPCTechSpring2025MovieApp.Client.HttpRepo;
using HPCTechSpring2025MovieApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Notifications;
using System.Net.Http.Json;

namespace HPCTechSpring2025MovieApp.Client.Pages;

public partial class Search
{
    [Inject]
    public HttpClient Http { get; set; }
    [Inject]
    public IJSRuntime js { get; set; }
    [Inject]
    public ILogger<Search> Logger { get; set; }
    //[Inject]
    //public IUserMoviesHttpRepository UserMoviesHttpRepository { get; set; }
    private string searchTerm;
    public MovieSearchResultDto searchResult { get; set; }
    public List<MovieSearchResultItemDto> OMDBMovies { get; set; } = new();
    public MovieSearchResultItemDto? SelectedMovie { get; set; } = null;

    private bool IsClient { get; set; }
    private SfToast ToastObj;
    private string toastContent = string.Empty;
    private string toastCSS = "e-toast-success";
    public MovieDto? omdbMovie { get; set; }
    private int page { get; set; } = 1;
    private int totalItems { get; set; } = 0;


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            IsClient = js is not null &&
                       js.GetType().Name.Contains("WebAssemblyJSRuntime");

        }
    }

    public async Task GetSelectedRow(RowSelectEventArgs<MovieSearchResultItemDto> args)
    {
        SelectedMovie = args.Data;
        omdbMovie = await Http.GetFromJsonAsync<MovieDto>($"api/GetMovie?imdbID={SelectedMovie.imdbID}");

    }

    public async Task PageClick(PagerItemClickEventArgs args)
    {
        page = args.CurrentPage;
        await SearchOMDB();
    }

    private async Task SearchOMDB()
    {
        SelectedMovie = null;
        searchResult = await Http.GetFromJsonAsync<MovieSearchResultDto>($"api/SearchMovies?searchTerm={searchTerm}&page={page}");
        if (searchResult?.Search?.Any() ?? false)
        {
            totalItems = Int32.Parse(searchResult.totalResults);
            OMDBMovies = searchResult.Search;
        }
    }

    public async Task ToolbarClickHandler(ClickEventArgs args)
    {
        if (args.Item.Id == "GridMovieAdd")
        {
            await AddMovie();
        }
    }

    public async Task AddMovie()
    {
        if (SelectedMovie is null)
        {
            if (IsClient)
            {
                toastContent = "Select a movie first";
                toastCSS = "e-toast-warning";
                StateHasChanged();
                await Task.Delay(100);
                await ToastObj.ShowAsync();
            } else
            {
                js.InvokeVoidAsync("alert", "Select a movie first!!");
            }
            return;
        }
        MovieDto newMovie = new MovieDto()
        {
            imdbID = SelectedMovie.imdbID
        };

        // grab problem response
        var res = await Http.PostAsJsonAsync($"api/add-movie", newMovie);

        if (res.IsSuccessStatusCode)
        {

            // toast response success
            if (IsClient)
            {
                toastContent = "Movie added";
                toastCSS = "e-toast-success";
                StateHasChanged();
                await Task.Delay(100);
                await ToastObj.ShowAsync();
            } else
            {
                js.InvokeVoidAsync("alert", "Movie added");
            }
                
        } else
        {
            Logger.LogWarning("Error adding movie {Movie} to user favorites. " +
                "Logged at {Placeholder:MMMM dd, yyyy}", SelectedMovie.imdbID, DateTimeOffset.UtcNow);
            var problem = await res.Content.ReadFromJsonAsync<ProblemResponse>();
            // toast response failure
            if (IsClient)
            {
                toastContent = $"{problem?.Detail ?? "error occured"}";
                toastCSS = "e-toast-danger";
                StateHasChanged();
                await Task.Delay(100);
                await ToastObj.ShowAsync();
            } else
            {
                js.InvokeVoidAsync("alert", $"{problem?.Detail ?? "error occured"}");
            }
        }
    }
}
