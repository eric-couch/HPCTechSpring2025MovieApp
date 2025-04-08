using HPCTechSpring2025MovieApp.Shared;
using HPCTechSpring2025MovieApp.Models;
using Syncfusion.Blazor.Gantt.Internal;

namespace HPCTechSpring2025MovieApp.Services;

public interface IMovieService
{
    Task<MovieSearchResultDto>? SearchOMDBMovies(string searchTerm, int page = 1);
    Task<MovieDto>? GetOMDBMovie(string imdbID);
    Task<Movie>? GetMovieById(string imdbId);

    Task<Movie> AddMovie(MovieDto movie);

    Task<bool> AddMovieToUser(ApplicationUserMovie applicationUserMovie);
}
