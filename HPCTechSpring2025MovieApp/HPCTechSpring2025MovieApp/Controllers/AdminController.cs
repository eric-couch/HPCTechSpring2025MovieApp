using HPCTechSpring2025MovieApp.Data;
using HPCTechSpring2025MovieApp.Shared;
using HPCTechSpring2025MovieApp.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.XlsIO.Implementation;

namespace HPCTechSpring2025MovieApp.Controllers;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("api/GetUsers")]
    public async Task<IActionResult> GetUsers()
    {
        var users = (from u in _context.Users
                     let query = (from ur in _context.Set<IdentityUserRole<string>>()
                                  where ur.UserId == u.Id
                                  join r in _context.Roles on ur.RoleId equals r.Id
                                  select r.Name).ToList()
                     select new UserEditDto
                     {
                         Id = u.Id,
                         UserName = u.UserName,
                         Email = u.Email,
                         Admin = query.Contains("Admin"),
                         EmailConfirmed = u.EmailConfirmed,
                         FirstName = u.FirstName,
                         LastName = u.LastName
                     }).ToList();

        DataResponse<List<UserEditDto>> dataResponse = new DataResponse<List<UserEditDto>>
        {
            Data = users,
            Succeeded = true,
            Message = "Users retrieved successfully"
        };

        return Ok(dataResponse);
    }

    [HttpPost("api/update-user")]
    public async Task<IActionResult> UpdateUser([FromBody] UserEditDto user)
    {
        var userToUpdate = await (from u in _context.Users where u.Id == user.Id select u).FirstOrDefaultAsync();
        if (userToUpdate is null)
        {
            return NotFound();
        }

        userToUpdate.FirstName = user.FirstName;
        userToUpdate.LastName = user.LastName;
        userToUpdate.Email = user.Email;
        userToUpdate.EmailConfirmed = user.EmailConfirmed;

        var roles = await (from r in _context.Roles
                           join ur in _context.UserRoles on r.Id equals ur.RoleId
                           where ur.UserId == user.Id
                           select r.Name).ToListAsync();

        
        if (!user.Admin == roles.Contains("Admin"))
        {
            await ToggleAdmin(userToUpdate.Id);
        }
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("api/ToggleEmailConfirmed")]
    public async Task<IActionResult> ToggleEmailConfirmed(string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound(new Response
            {
                Succeeded = false,
                Message = "User not found"
            });
        }
        user.EmailConfirmed = !user.EmailConfirmed;
        await _context.SaveChangesAsync();
        return Ok(new Response
        {
            Succeeded = true,
            Message = "Email confirmation status updated successfully"
        });
    }


    [HttpGet("api/ToggleAdmin")]
    public async Task<IActionResult> ToggleAdmin(string userId)
    {
        var user = _context.Users.Find(userId);
        if (user == null)
        {
            return NotFound(new Response { Succeeded = false, Message = "user not found" });
        }

        var roles = await (from r in _context.Roles
                           join ur in _context.UserRoles on r.Id equals ur.RoleId
                           where ur.UserId == user.Id
                           select r.Name).ToListAsync();

        if (roles.Contains("Admin"))
        {
            var userRoleToRemove = await (from ur in _context.UserRoles
                                      join r in _context.Roles on ur.RoleId equals r.Id
                                      where ur.UserId == user.Id
                                      && r.Name == "Admin"
                                      select ur).FirstOrDefaultAsync();

            _context.UserRoles.Remove(userRoleToRemove);
            await _context.SaveChangesAsync();
            return Ok(new Response(true, $"Admin role removed from user {user.UserName}"));
            
        }
        else
        {
            var roleToAdd = await (from r in _context.Roles
                                   where r.Name == "Admin"
                                   select r).FirstOrDefaultAsync();

            _context.UserRoles.Add(new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = roleToAdd.Id
            });

            await _context.SaveChangesAsync();
            return Ok(new Response(true, $"Admin role added to user {user.UserName}"));
        }

    }

}
