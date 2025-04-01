using HPCTechSpring2025MovieApp.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace HPCTechSpring2025MovieApp.Client.Pages;

public partial class Search
{
    [Inject]
    public HttpClient Http { get; set; }
    private string searchTerm;
    public MovieSearchResultDto searchResult { get; set; }

    private async Task SearchOMDB()
    {
        searchResult = await Http.GetFromJsonAsync<MovieSearchResultDto>($"api/SearchMovies?searchTerm={searchTerm}");
    }
}
