using HPCTechSpring2025MovieApp.Controllers;
using HPCTechSpring2025MovieApp.Data;
using HPCTechSpring2025MovieApp.Services;
using HPCTechSpring2025MovieApp.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace HPCTechSpring2025MovieApp.Test;

public class Tests
{
    public readonly Mock<IMovieService> _movieServiceMock = new();
    public readonly Mock<IUserService> _userServiceMock = new();
    public readonly Mock<ILogger<MovieController>> _loggerMock = new();

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetOMDBMovie_ShouldReturnMovieDto_WhereMovieExists()
    {
        // Arrange
        string omdbId = "tt0816692";
        string userName = "eric.couch@cognizant.com";
        MovieDto movieDto = new MovieDto()
        {
            imdbID = omdbId,
            Title = "Interstellar",
            Year = "2014",
            Rated = "PG-13",
            Released = "07 Nov 2014",
            Runtime = "169 min",
            Genre = "Adventure, Drama, Sci-Fi",
            Director = "Christopher Nolan",
            Writer = "Jonathan Nolan, Christopher Nolan",
            Actors = "Matthew McConaughey, Anne Hathaway, Jessica Chastain",
            Plot = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.",
            Language = "English",
            Country = "USA, UK",
            Awards = "Won 1 Oscar. 43 wins & 148 nominations total",
            Poster = "https://m.media-amazon.com/images/M/MV5BMjIxMjY0NjY4MF5BMl5BanBnXkFtZTgwNTQ2MTQ3MjE@._V1_SX300.jpg",
            Metascore = "74",
            imdbRating = "8.6",
            imdbVotes = "1,000,000",
            Type = "movie",
            DVD = "07 Apr 2015",
            BoxOffice = "$188,020,017",
            Production = "Warner Bros., Syncopy, Lynda Obst Productions",
            Website = "N/A",
            Response = "True"
        };

        ApplicationUser applicationUser = new ApplicationUser()
        {
            Id = "5c47dc40-1fc2-49c5-a4bf-79b1230135a6",
            UserName = userName,
            Email = userName,
            NormalizedEmail = userName.ToUpper(),
            NormalizedUserName = userName.ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D"),
            PasswordHash = "hashedPassword"
        };
        _movieServiceMock.Setup(x => x.GetOMDBMovie(omdbId))
                        .ReturnsAsync(movieDto);

        _userServiceMock.Setup(x => x.GetUserByUserNameAsync(userName))
                        .ReturnsAsync(applicationUser);

        _loggerMock.Setup(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()));


        MovieController movieController = new MovieController(  _movieServiceMock.Object, 
                                                                _userServiceMock.Object,
                                                                _loggerMock.Object);

        // Act
        var response = await movieController.GetOMDBMovie(omdbId);
        var okResult = response as OkObjectResult;
        MovieDto movie = okResult?.Value as MovieDto;

        // Assert 
        Assert.IsNotNull(okResult);
        Assert.IsInstanceOf<OkObjectResult>(okResult);
        Assert.IsNotNull(okResult.Value);
        Assert.IsInstanceOf<MovieDto>(movie);
        Assert.That(movie.imdbID, Is.EqualTo(omdbId));
        Assert.That(movie.Title, Is.EqualTo("Interstellar"));
        Assert.That(movie.Year, Is.EqualTo("2014"));
        Assert.That(movie.Rated, Is.EqualTo("PG-13"));
        Assert.That(movie.Released, Is.EqualTo("07 Nov 2014"));
        Assert.That(movie.Runtime, Is.EqualTo("169 min"));
    }
}