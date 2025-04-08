using HPCTechSpring2025MovieApp.Shared;
using HPCTechSpring2025MovieApp.Data;
using HPCTechSpring2025MovieApp.Models;
using Microsoft.AspNetCore.Identity;

namespace HPCTechSpring2025MovieApp.Services;

public class MovieService : IMovieService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly ApplicationDbContext _context;
    private readonly string OMDBAPIUrl = "http://www.omdbapi.com/?";

    public MovieService(HttpClient httpClient,
                            IConfiguration config,
                            ApplicationDbContext context)
    {
        _httpClient = httpClient;
        _config = config;
        _context = context;
    }

    public async Task<MovieSearchResultDto>? SearchOMDBMovies(string searchTerm, int page = 1)
    {
        try
        {
            string OMDBAPIKey = _config["Movies:OMDBAPIKey"];
            var searchResult = await _httpClient.GetFromJsonAsync<MovieSearchResultDto>($"{OMDBAPIUrl}apikey={OMDBAPIKey}&s={searchTerm}&page={page}");
            return searchResult;
        }
        catch (Exception ex)
        {
            // TODO: add logging and proper error handling
            return null;
        }
    }

    public async Task<MovieDto>? GetOMDBMovie(string imdbID)
    {
        try
        {
            string OMDBAPIKey = _config["Movies:OMDBAPIKey"];
            MovieDto movie = await _httpClient.GetFromJsonAsync<MovieDto>($"{OMDBAPIUrl}apikey={OMDBAPIKey}&i={imdbID}");
            return movie;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<Movie> AddMovie(MovieDto omdbmovie)
    {
        
        await _context.Movies.AddAsync(new Movie
        {
            imdbID = omdbmovie.imdbID,
            Title = omdbmovie.Title,
            Year = omdbmovie.Year,
            Rated = omdbmovie.Rated,
            Released = omdbmovie.Released,
            Runtime = omdbmovie.Runtime,
            Genre = omdbmovie.Genre,
            Director = omdbmovie.Director,
            Writer = omdbmovie.Writer,
            Actors = omdbmovie.Actors,
            Plot = omdbmovie.Plot,
            Language = omdbmovie.Language,
            Country = omdbmovie.Country,
            Awards = omdbmovie.Awards,
            Poster = omdbmovie.Poster,
            Metascore = omdbmovie.Metascore,
            imdbRating = omdbmovie.imdbRating,
            imdbVotes = omdbmovie.imdbVotes,
            Type = omdbmovie.Type,
            DVD = omdbmovie.DVD,
            BoxOffice = omdbmovie.BoxOffice,
            Production = omdbmovie.Production,
            Website = omdbmovie.Website,
            Response = omdbmovie.Response
        });
        await _context.SaveChangesAsync();
        return await _context.Movies.FindAsync(omdbmovie.imdbID);
    }

    public async Task<Movie>? GetMovieById(string imdbId)
    {
        try
        {
            var movie = await _context.Movies.FindAsync(imdbId);
            return movie;
        } catch (Exception) {
            return null;
        }
    }

    public async Task<bool> AddMovieToUser(ApplicationUserMovie applicationUserMovie)
    {
        try
        {
            // check to see if association exists
            bool associationExists = _context.Set<ApplicationUserMovie>()
                                                .Any(aum => aum.MovieId == applicationUserMovie.MovieId &&
                                                aum.ApplicationUserId == applicationUserMovie.ApplicationUserId);

            if (associationExists)
            {
                return false;
            }

            _context.Add(applicationUserMovie);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            // TODO: ADD logging and error handling
            return false;
        }
    }
}
