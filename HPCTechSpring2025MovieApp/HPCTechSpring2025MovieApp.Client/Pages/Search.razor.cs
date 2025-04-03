using HPCTechSpring2025MovieApp.Shared;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using System.Net.Http.Json;

namespace HPCTechSpring2025MovieApp.Client.Pages;

public partial class Search
{
    [Inject]
    public HttpClient Http { get; set; }
    private string searchTerm;
    public MovieSearchResultDto searchResult { get; set; }
    public List<MovieSearchResultItemDto> OMDBMovies { get; set; } = new();
    public MovieSearchResultItemDto SelectedMovie { get; set; }
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
            // notify the user with toast response
            return;
        }
        MovieDto newMovie = new MovieDto()
        {
            imdbID = SelectedMovie.imdbID
        };

        var res = await Http.PostAsJsonAsync($"api/add-movie", newMovie);

        if (res.IsSuccessStatusCode)
        {
            // toast response success
        } else
        {
            // toast response failure
        }

    }
}
