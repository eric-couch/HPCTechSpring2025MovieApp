using Microsoft.AspNetCore.Mvc;
using HPCTechSpring2025MovieApp.Shared;
using HPCTechSpring2025MovieApp.Data;
using Microsoft.AspNetCore.Identity;
using HPCTechSpring2025MovieApp.Models;
using HPCTechSpring2025MovieApp.Services;
using Microsoft.OpenApi.Services;
using Syncfusion.Blazor.PivotView;

namespace HPCTechSpring2025MovieApp.Controllers;

public class MovieController : Controller
{
    private readonly IMovieService _movieService;
    private readonly IUserService _userService;

    public MovieController(IMovieService movieService, IUserService userService)
    {
        _movieService = movieService;
        _userService = userService;
    }

    [HttpGet]
    [Route("api/SearchMovies")]
    public async Task<IActionResult> SearchOMDBMovies([FromQuery] string searchTerm, int page = 1)
    {
        try
        {
            var searchResult = await _movieService.SearchOMDBMovies(searchTerm, page);
            if (searchResult?.Search?.Any() ?? false)
            {
                return Ok(searchResult);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest(ex.Message);
        }

        
    }

    [HttpGet]
    [Route("api/GetMovie")]
    public async Task<IActionResult> GetOMDBMovie(string imdbID)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(imdbID))
            {
                return BadRequest();
            }
            MovieDto movie = await _movieService.GetOMDBMovie(imdbID);

            if (movie != null)
            {
                return Ok(movie);
            }
            else
            {
                return NotFound();
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return NotFound();
        }
        
    }

    [HttpPost("api/add-movie")]
    public async Task<IActionResult> AddMovie([FromBody] MovieDto movieDto)
    {
        var userName = User.Identity?.Name;
        //var user = await _userManager.FindByNameAsync(userName);
        var user = await _userService.GetUserByUserNameAsync(userName);

        if (user is null)
        {
            return NotFound();
        }

        //var movie = _context.Movies.Find(movieDto.imdbID);
        var movie = await _movieService.GetMovieById(movieDto.imdbID);


        if (movie == null)
        {
            MovieDto omdbMovie = await _movieService.GetOMDBMovie(movieDto.imdbID);
            movie = await _movieService.AddMovie(omdbMovie);
        }

        var applicationUserMovie = new ApplicationUserMovie
        {
            ApplicationUserId = user.Id,
            MovieId = movie.imdbID,
            ApplicationUser = user,
            Movie = movie
        };
        try
        {
            bool success = await _movieService.AddMovieToUser(applicationUserMovie);
            if (success)
            {
                return Ok(movie);   
            }
            else
            {
                var problem = new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Movie already exists in user's favorites.",
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path
                };

                return BadRequest(problem);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
