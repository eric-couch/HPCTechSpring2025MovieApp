using Microsoft.AspNetCore.Mvc;
using HPCTechSpring2025MovieApp.Shared;
using HPCTechSpring2025MovieApp.Data;
using Microsoft.AspNetCore.Identity;
using HPCTechSpring2025MovieApp.Models;

namespace HPCTechSpring2025MovieApp.Controllers;

public class MovieController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly string OMDBAPIUrl = "http://www.omdbapi.com/?";

    public MovieController( HttpClient httpClient, 
                            IConfiguration config,
                            UserManager<ApplicationUser> userManager, 
                            ApplicationDbContext context)
    {
        _httpClient = httpClient;
        _config = config;
        _userManager = userManager;
        _context = context;
    }

    //// /api/Movie
    //[HttpGet]
    //public async Task<IActionResult> ListMovies()
    //{
    //    return Ok("ListMovies");
    //}

    //// /api/Movie/1
    //[HttpGet("{id}")]
    //public async Task<IActionResult> GetMovie(int id)
    //{
    //    return Ok("GetMovie");
    //}

    [HttpGet]
    [Route("api/SearchMovies")]
    public async Task<IActionResult> SearchOMDBMovies([FromQuery] string searchTerm, int page = 1)
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

    //[HttpPost]
    //[Route("api/SearchMovies/{id?:int}")]
    //[Route("api/SearchOMDBMoviesPost")]
    //public async Task<IActionResult> SearchOMDBMoviesPost([FromBody] MovieDto movie)
    //{
    //    string OMDBAPIKey = _config["Movies:OMDBAPIKey"];
    //    var searchResult = await _httpClient.GetFromJsonAsync<MovieSearchResultDto>($"{OMDBAPIUrl}apikey={OMDBAPIKey}&s={searchTerm}");
    //    if (searchResult?.Search?.Any() ?? false)
    //    {
    //        return Ok(searchResult);
    //    }
    //    else
    //    {
    //        return NotFound();
    //    }
    //}

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

    [HttpPost("api/add-movie")]
    public async Task<IActionResult> AddMovie([FromBody] MovieDto movieDto)
    {
        var userName = User.Identity?.Name;
        var user = await _userManager.FindByNameAsync(userName);

        if (user is null) 
        {
            return NotFound();
        }

        var movie = _context.Movies.Find(movieDto.imdbID);

        string OMDBAPIKey = _config["Movies:OMDBAPIKey"];
        MovieDto omdbmovie = await _httpClient.GetFromJsonAsync<MovieDto>($"{OMDBAPIUrl}apikey={OMDBAPIKey}&i={movieDto.imdbID}");

        if (movie == null)
        {
            await _context.Movies.AddAsync(new Movie
            {
                imdbID = omdbmovie.imdbID,
                Title = omdbmovie.Title,
                Year = omdbmovie.Year,
                Rated = omdbmovie.Rated,
                Released = omdbmovie.Released,
                Runtime = omdbmovie.Runtime,
                Genre = omdbmovie.Genre,
                Director = omdbmovie.Director,
                Writer = omdbmovie.Writer,
                Actors = omdbmovie.Actors,
                Plot = omdbmovie.Plot,
                Language = omdbmovie.Language,
                Country = omdbmovie.Country,
                Awards = omdbmovie.Awards,
                Poster = omdbmovie.Poster,
                Metascore = omdbmovie.Metascore,
                imdbRating = omdbmovie.imdbRating,
                imdbVotes = omdbmovie.imdbVotes,
                Type = omdbmovie.Type,
                DVD = omdbmovie.DVD,
                BoxOffice = omdbmovie.BoxOffice,
                Production = omdbmovie.Production,
                Website = omdbmovie.Website,
                Response = omdbmovie.Response
            });
            movie = _context.Movies.Find(movieDto.imdbID);
        }

        var applicationUserMovie = new ApplicationUserMovie
        {
            ApplicationUserId = user.Id,
            MovieId = movie.imdbID
        };

        if (user.ApplicationUserMovies.Any(m => m.Movie.imdbID == movie.imdbID && m.ApplicationUserId == user.Id))
        {
            return BadRequest("Movie already exists in user's favorites.");
        } else
        {
            user.ApplicationUserMovies.Add(applicationUserMovie);
            await _context.SaveChangesAsync();
            return Ok("Movie added to favorites.");
        }

    }

}
