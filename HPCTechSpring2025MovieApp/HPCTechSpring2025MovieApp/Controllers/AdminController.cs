using HPCTechSpring2025MovieApp.Data;
using HPCTechSpring2025MovieApp.Shared;
using HPCTechSpring2025MovieApp.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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



}
