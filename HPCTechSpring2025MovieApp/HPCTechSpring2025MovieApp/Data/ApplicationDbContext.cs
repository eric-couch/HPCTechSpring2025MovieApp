using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HPCTechSpring2025MovieApp.Models;

namespace HPCTechSpring2025MovieApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Rating> Ratings => Set<Rating>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUserMovie>()
            .HasKey(aum => aum.Id);

        builder.Entity<ApplicationUserMovie>()
            .HasOne(aum => aum.ApplicationUser)
            .WithMany(u => u.ApplicationUserMovies)
            .HasForeignKey(aum => aum.ApplicationUserId);

        builder.Entity<ApplicationUserMovie>()
            .HasOne(aum => aum.Movie)
            .WithMany(m => m.ApplicationUserMovies)
            .HasForeignKey(aum => aum.MovieId);

    }
}
