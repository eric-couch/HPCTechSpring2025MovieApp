using Microsoft.AspNetCore.Mvc;
using HPCTechSpring2025MovieApp.Shared;

namespace HPCTechSpring2025MovieApp.Controllers;

public class MovieController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly string OMDBAPIUrl = "http://www.omdbapi.com/?";

    public MovieController(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }


    [HttpGet]
    [Route("api/SearchMovies")]
    public async Task<IActionResult> SearchOMDBMovies(string searchTerm, int page = 1)
    {
        string OMDBAPIKey = _config["Movies:OMDBAPIKey"];
        var searchResult = await _httpClient.GetFromJsonAsync<MovieSearchResultDto>($"{OMDBAPIUrl}apikey={OMDBAPIKey}&s={searchTerm}&page={page}");
        if (searchResult?.Search?.Any() ?? false)
        {
            return Ok(searchResult);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Route("api/GetMovie")]
    public async Task<IActionResult> GetOMDBMovie(string imdbID)
    {
        string OMDBAPIKey = _config["Movies:OMDBAPIKey"];
        MovieDto movie = await _httpClient.GetFromJsonAsync<MovieDto>($"{OMDBAPIUrl}apikey={OMDBAPIKey}&i={imdbID}");
        if (movie != null) { 
            return Ok(movie);
        } else
        {
            return NotFound();
        }
    }
}
