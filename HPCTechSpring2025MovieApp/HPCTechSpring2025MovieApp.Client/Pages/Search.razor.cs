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

    private string searchTerm;
    public MovieSearchResultDto searchResult { get; set; }
    public List<MovieSearchResultItemDto> OMDBMovies { get; set; } = new();
    public MovieSearchResultItemDto SelectedMovie { get; set; }

    //private SfToast ToastObj;
    //private string toastContent = string.Empty;
    //private string toastCSS = "e-toast-success";
    public MovieDto? omdbMovie { get; set; }
    private int page { get; set; } = 1;
    private int totalItems { get; set; } = 0;

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
            //toastContent = "Select a movie first";
            //toastCSS = "e-toast-warning";
            //await ToastObj.ShowAsync();
            js.InvokeVoidAsync("alert", "Select a movie first!!");
            return;
        }
        MovieDto newMovie = new MovieDto()
        {
            imdbID = SelectedMovie.imdbID
        };

        // grab problem response
        var res = await Http.PostAsJsonAsync($"api/add-movie", newMovie);

        var problem = await res.Content.ReadFromJsonAsync<ProblemResponse>();

        if (res.IsSuccessStatusCode)
        {
            // toast response success
            js.InvokeVoidAsync("alert", "good!");
        } else
        {
            // toast response failure
            js.InvokeVoidAsync("alert", $"{problem?.Detail ?? "error"}");
        }

    }
}
