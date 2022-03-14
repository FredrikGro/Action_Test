using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using filmstudion.api.Models;
using Microsoft.EntityFrameworkCore;

namespace filmstudion.api.Data
{
  public class FilmRepository : IFilmRepository
  {
    readonly APIDbContext context;
    private readonly IFilmCopyRepository filmCopyRepository;
    private readonly IFilmStudioRepository filmStudioRepository;

    public FilmRepository(APIDbContext context, IFilmCopyRepository filmCopyRepository, IFilmStudioRepository filmStudioRepository)
    {
      this.context = context;
      this.filmCopyRepository = filmCopyRepository;
      this.filmStudioRepository = filmStudioRepository;
    }
    public IEnumerable<Film> GetAllFilms
    {
      get
      {
        return this.context.Films.Include(e => e.FilmCopies);
      }
    }

    public IEnumerable<Film> GetAllFilmsAnonymous
    {
      get
      {
        return this.context.Films;
      }
    }

    public async Task<Film> CreateFilmAsync(CreateFilmModel createFilmModel)
    {
      var film = new Film
      {
        Name = createFilmModel.Name,
        Country = createFilmModel.Country,
        Director = createFilmModel.Director,
        ReleaseDate = createFilmModel.ReleaseDate,
        FilmId = Guid.NewGuid().ToString()
      };
      var filmCopies = new List<FilmCopy>();
      for (int i = 0; i < createFilmModel.NumberOfCopies; i++)
      {
        var copy = new FilmCopy
        {
          FilmCopyId = Guid.NewGuid().ToString(),
          RentedOut = false,
          FilmId = film.FilmId
        };
        filmCopies.Add(copy);
      }
      await context.Films.AddAsync(film);
      await context.FilmCopies.AddRangeAsync(filmCopies);
      context.SaveChanges();
      return film;
    }

    public async Task<Film> GetFilmFromIdAsync(string Id)
    {
      return await context.Films.Include(f => f.FilmCopies).FirstOrDefaultAsync(f => f.FilmId == Id);
    }
    public async Task<Film> GetFilmAnonymousFromIdAsync(string Id)
    {
      return await context.Films.FirstOrDefaultAsync(f => f.FilmId == Id);
    }

    public string GetFilmStudioIdFromIdentityName(string IdentityId)
    {
      try
      {
        string FilmStudioId = context.Users.Include(u => u.filmStudio).FirstOrDefault(u => u.UserId == IdentityId).filmStudio.FilmStudioId;
        return FilmStudioId;
      }
      catch
      {
        return null;
      }
    }

    public async Task<bool> HasAvailableCopyAsync(string FilmId)
    {
      var film = await GetFilmFromIdAsync(FilmId);
      if (film.FilmCopies.FirstOrDefault(f => f.RentedOut == false) != null)
      {
        return true;
      }

      return false;
    }

    public async Task<bool> NoOfFilmCopiesUpdateIsValid(string filmId, int noOfCopies)
    {
      var film = await GetFilmFromIdAsync(filmId);
      int currentFilmsTotal = film.FilmCopies.Count();
      int currentFilmsAvailable = film.FilmCopies.Select(c => c.RentedOut == false).Count();
      if (currentFilmsAvailable > noOfCopies)
      {
        return true;
      }
      if (noOfCopies >= currentFilmsTotal)
      {
        return true;
      }

      return false;
    }

    public async Task<FilmCopy> RentCopyAsync(string filmStudioId, string filmId)
    {
      var film = await GetFilmFromIdAsync(filmId);
      var copy = film.FilmCopies.FirstOrDefault(c => c.RentedOut == false);

      copy.RentedOut = true;
      copy.FilmStudioId = filmStudioId;

      context.FilmCopies.Update(copy);
      context.SaveChanges();

      return filmCopyRepository.GetFilmCopyFromId(copy.FilmCopyId);
    }

    public FilmCopy ReturnFilmCopyAsync(string filmStudioId, string filmId)
    {
      var filmcopy = filmStudioRepository.GetFilmStudioFromId(filmStudioId)?.RentedFilmCopies.FirstOrDefault(c => c.FilmId == filmId);

      filmcopy.RentedOut = false;
      filmcopy.FilmStudioId = null;
      context.FilmCopies.Update(filmcopy);
      context.SaveChanges();

      return filmCopyRepository.GetFilmCopyFromId(filmcopy.FilmCopyId);

    }

    public async Task<Film> UpdateFilmAsync(string Id, UpdateFilmModel updateFilmModel)
    {
      var film = await GetFilmFromIdAsync(Id);
      film.Name = updateFilmModel.Name;
      film.ReleaseDate = updateFilmModel.ReleaseDate;
      film.Director = updateFilmModel.Director;
      film.Country = updateFilmModel.Country;

      context.Films.Update(film);
      context.SaveChanges();
      return film;
    }

    public async Task<Film> UpdateNoOfFilmCopies(string filmId, int noOfCopies)
    {
      var film = await GetFilmFromIdAsync(filmId);

      int originalCopies = film.FilmCopies.Count();
      int copiesDeleted = 0;

      if (noOfCopies < originalCopies)
      {
        film.FilmCopies.FindAll(c => c.RentedOut == false).ToList().ForEach(c =>
              {
                if ((originalCopies - copiesDeleted) > noOfCopies)
                {
                  if (!c.RentedOut)
                  {
                    filmCopyRepository.DeleteFilmCopyFromId(c.FilmCopyId);
                    copiesDeleted++;
                  }
                }
                return;
              });
      }

      if (noOfCopies > originalCopies)
      {
        int copiesToCreate = noOfCopies - originalCopies;

        for (int i = 0; i < copiesToCreate; i++)
        {
          filmCopyRepository.CreateFilmCopy(filmId);
        }
      }

      return await GetFilmFromIdAsync(filmId);
    }
  }
}