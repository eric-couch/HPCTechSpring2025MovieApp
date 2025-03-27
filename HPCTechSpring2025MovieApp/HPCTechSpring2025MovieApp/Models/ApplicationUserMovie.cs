using HPCTechSpring2025MovieApp.Data;
using System.ComponentModel.DataAnnotations;

namespace HPCTechSpring2025MovieApp.Models;

public class ApplicationUserMovie
{
    public int Id { get; set; }
    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public string MovieId { get; set; }
    public Movie Movie { get; set; }
}
