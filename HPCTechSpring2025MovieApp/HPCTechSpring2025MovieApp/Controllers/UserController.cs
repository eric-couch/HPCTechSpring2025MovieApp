using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HPCTechSpring2025MovieApp.Models;
using HPCTechSpring2025MovieApp.Data;
using Microsoft.EntityFrameworkCore;
using HPCTechSpring2025MovieApp.Shared;
using Microsoft.AspNetCore.Authorization;

namespace HPCTechSpring2025MovieApp.Controllers;


public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> UserManager;
    private readonly ApplicationDbContext Context;

    public UserController(UserManager<ApplicationUser> _userManager, ApplicationDbContext _context)
    {
        UserManager = _userManager;
        Context = _context;
    }

    [HttpGet]
    [Route("api/User")]
    public async Task<IActionResult> GetMovies()
    {
        var userName = User.Identity?.Name;

        if (userName == null)
        {
            return Unauthorized();
        }

        var user = await UserManager.FindByNameAsync(userName);

        if (user == null)
        {
            return NotFound("User not found.");
        }
        else
        {
            //var userDto = await Context.Users
            //                .Where(u => u.Id == user.Id)
            //                .Select(u => new UserDto
            //                {
            //                    Id = u.Id,
            //                    FirstName = u.FirstName,
            //                    LastName = u.LastName,
            //                    FavoriteMovies = u.ApplicationUserMovies.Select(aum => aum.Movie).ToList()
            //                }).FirstOrDefaultAsync();

            var userDto = await (from u in Context.Users
                                 where u.Id == user.Id
                                 select new UserDto
                                 {
                                     Id = u.Id,
                                     FirstName = u.FirstName,
                                     LastName = u.LastName,
                                     FavoriteMovies = u.ApplicationUserMovies.Select(aum =>
                                           new MovieDto
                                           {
                                               imdbID = aum.Movie.imdbID,
                                               Title = aum.Movie.Title,
                                               Year = aum.Movie.Year,
                                               Rated = aum.Movie.Rated,
                                               Released = aum.Movie.Released,
                                               Runtime = aum.Movie.Runtime,
                                               Genre = aum.Movie.Genre,
                                               Director = aum.Movie.Director,
                                               Writer = aum.Movie.Writer,
                                               Actors = aum.Movie.Actors,
                                               Plot = aum.Movie.Plot,
                                               Language = aum.Movie.Language,
                                               Country = aum.Movie.Country,
                                               Awards = aum.Movie.Awards,
                                               Poster = aum.Movie.Poster,
                                               Metascore = aum.Movie.Metascore,
                                               imdbRating = aum.Movie.imdbRating,
                                               imdbVotes = aum.Movie.imdbVotes,
                                               Type = aum.Movie.Type,
                                               DVD = aum.Movie.DVD,
                                               BoxOffice = aum.Movie.BoxOffice,
                                               Production = aum.Movie.Production,
                                               Website = aum.Movie.Website,
                                               Response = aum.Movie.Response
                                           }
                                         ).ToList()
                                 }).FirstOrDefaultAsync();

            if (userDto == null)
            {
                return NotFound("User not found.");
            }
            else
            {
                return Ok(userDto);
            }
        }
    }
}
