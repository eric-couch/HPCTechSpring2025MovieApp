using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HPCTechSpring2025MovieApp.Models;
using HPCTechSpring2025MovieApp.Data;
using Microsoft.EntityFrameworkCore;
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
                                     FavoriteMovies = u.ApplicationUserMovies.Select(aum => aum.Movie).ToList()
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
