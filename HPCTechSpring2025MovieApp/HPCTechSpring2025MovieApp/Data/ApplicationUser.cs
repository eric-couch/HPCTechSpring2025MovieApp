using Microsoft.AspNetCore.Identity;
using HPCTechSpring2025MovieApp.Models;

namespace HPCTechSpring2025MovieApp.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public List<ApplicationUserMovie> ApplicationUserMovies { get; set; } = new List<ApplicationUserMovie>();

}
