using System;
using filmstudion.api.Models;
using Microsoft.EntityFrameworkCore;

namespace filmstudion.api.Data
{
  public class APIDbContext : DbContext
  {
    public APIDbContext(DbContextOptions<APIDbContext> options) : base(options)
    {
      this.Database.EnsureCreated();
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Film> Films { get; set; }
    public DbSet<FilmCopy> FilmCopies { get; set; }
    public DbSet<FilmStudio> FilmStudios { get; set; }
    public DbSet<FilmStudioPresidentContact> FilmStudioPresidentContact { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);


      //seed users
      modelBuilder.Entity<User>().HasData(new User { Username = "studio1", FilmStudioId = "studio1", UserId = "user1", Role = Role.FilmStudio, Password = "p@ssw0rd" });
      modelBuilder.Entity<User>().HasData(new User { Username = "admin", UserId = "user2", Role = Role.Admin, Password = "p@ssw0rd" });

      
      modelBuilder.Entity<FilmStudio>().HasData(new FilmStudio { Name = "studio1", FilmStudioId = "studio1", City = "Göteborg", FilmStudioPresidentContactId = "pres1" });

      modelBuilder.Entity<FilmStudioPresidentContact>().HasData(new FilmStudioPresidentContact { FilmStudioPresidentContactId = "pres1", Name = "Jörgen Karlsson", Address = "Blåa vägen 7, 88290, Sollentuna", PhoneNumber = "0340651492" });

      //seed Films
      modelBuilder.Entity<Film>().HasData(new Film { Name = "Die Hard", Country = "USA", Director = "Ben Benston", FilmId = "abc123", ReleaseDate = new DateTime(1995, 1, 18) });
      modelBuilder.Entity<Film>().HasData(new Film { Name = "Det sjätte inseglet", Country = "Sverige", Director = "Ingmar Bergman", FilmId = "def456", ReleaseDate = new DateTime(1960, 1, 18) });
      modelBuilder.Entity<Film>().HasData(new Film { Name = "Nicke nyfiken", Country = "Sverige", Director = "Apan Apansson", FilmId = "ghi789", ReleaseDate = new DateTime(1975, 1, 18) });


      //seed Filmcopies
      modelBuilder.Entity<FilmCopy>().HasData(new FilmCopy { FilmCopyId = "a1", FilmId = "abc123", RentedOut = false });
      modelBuilder.Entity<FilmCopy>().HasData(new FilmCopy { FilmCopyId = "b2", FilmId = "abc123", RentedOut = false });
      modelBuilder.Entity<FilmCopy>().HasData(new FilmCopy { FilmCopyId = "c3", FilmId = "abc123", RentedOut = false });
      modelBuilder.Entity<FilmCopy>().HasData(new FilmCopy { FilmCopyId = "d4", FilmId = "abc123", RentedOut = false });

      modelBuilder.Entity<FilmCopy>().HasData(new FilmCopy { FilmCopyId = "e5", FilmId = "def456", RentedOut = false });
      modelBuilder.Entity<FilmCopy>().HasData(new FilmCopy { FilmCopyId = "f6", FilmId = "def456", RentedOut = false });
      modelBuilder.Entity<FilmCopy>().HasData(new FilmCopy { FilmCopyId = "g8", FilmId = "def456", RentedOut = false });
      modelBuilder.Entity<FilmCopy>().HasData(new FilmCopy { FilmCopyId = "H9", FilmId = "def456", RentedOut = true, FilmStudioId = "studio1" });

      modelBuilder.Entity<FilmCopy>().HasData(new FilmCopy { FilmCopyId = "I10", FilmId = "ghi789", RentedOut = true, FilmStudioId = "studio1" });





    }

  }
}