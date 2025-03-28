using HPCTechSpring2025MovieApp.Shared;

namespace HPCTechSpring2025MovieApp.Shared;

public class UserDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public List<MovieDto> FavoriteMovies { get; set; } = new List<MovieDto>();
}
