using HPCTechSpring2025MovieApp.Models;

namespace HPCTechSpring2025MovieApp.Data;

public class UserDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public List<Movie> FavoriteMovies { get; set; } = new List<Movie>();
}
