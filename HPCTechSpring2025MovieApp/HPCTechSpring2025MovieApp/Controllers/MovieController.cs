using Microsoft.AspNetCore.Mvc;
using HPCTechSpring2025MovieApp.Shared;

namespace HPCTechSpring2025MovieApp.Controllers;

public class MovieController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string OMDBAPIUrl = "http://www.omdbapi.com/?";
    private readonly string OMDBAPIKey = "apikey=86c39163";

    public MovieController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    [HttpGet]
    [Route("api/SearchMovies")]
    public async Task<IActionResult> SearchOMDBMovies(string searchTerm, int page = 1)
    {
        var searchResult = await _httpClient.GetFromJsonAsync<MovieSearchResultDto>($"{OMDBAPIUrl}{OMDBAPIKey}&s={searchTerm}");
        if (searchResult?.Search?.Any() ?? false)
        {
            return Ok(searchResult);
        }
        else
        {
            return NotFound();
        }
    }
}
