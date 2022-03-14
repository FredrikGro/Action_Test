using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using filmstudion.api.Helpers;
using filmstudion.api.Models;
using filmstudion.api.Services;
using Microsoft.EntityFrameworkCore;

namespace filmstudion.api.Data
{
  public class FilmStudioRepository : IFilmStudioRepository
  {
    private readonly APIDbContext context;
    private readonly IFilmCopyRepository filmCopyRepository;
    private readonly IUserService userService;

    public FilmStudioRepository(APIDbContext context, IFilmCopyRepository filmCopyRepository, IUserService UserService)
    {
      this.context = context;
      this.filmCopyRepository = filmCopyRepository;
      userService = UserService;
    }
    public IEnumerable<FilmStudio> GetAllFilmStudios
    {
      get
      {
        return context.FilmStudios.Include(s => s.filmStudioPresidentContact).Include(s => s.RentedFilmCopies);
      }
    }

    public IEnumerable<FilmStudio> GetAllFilmStudiosAnonymous
    {
      get
      {
        return context.FilmStudios;
      }
    }
    public bool FilmStudioExists(string FilmStudioId)
    {
      return context.FilmStudios.FirstOrDefault(s => s.FilmStudioId == FilmStudioId) != null;
    }

    public bool FilmStudioHasOngoingRentOfCopy(string filmStudioId, string filmId)
    {
      return GetFilmStudioFromId(filmStudioId).RentedFilmCopies.FirstOrDefault(c => c.FilmId == filmId) != null;
    }
    public FilmStudio GetFilmStudioFromId(string filmStudioId)
    {
      return context.FilmStudios.Include(s => s.RentedFilmCopies).Include(s => s.filmStudioPresidentContact).FirstOrDefault(s => s.FilmStudioId == filmStudioId);
    }
    public FilmStudio GetFilmStudioFromIdAnonymous(string requestedFilmStudioId)
    {
      return context.FilmStudios.FirstOrDefault(s => s.FilmStudioId == requestedFilmStudioId);
    }
    public async Task<User> RegisterFilmStudioAsync(FilmStudioRegisterModel registerModel)
    {
      var president = new FilmStudioPresidentContact
      {
        Address = registerModel.FilmStudioPresidentAddress,
        Name = registerModel.FilmStudioName,
        PhoneNumber = registerModel.FilmStudioPresidentPhoneNumber,
        FilmStudioPresidentContactId = Guid.NewGuid().ToString()
      };

      var studio = new FilmStudio
      {
        Name = registerModel.FilmStudioName,
        City = registerModel.FilmStudioCity,
        FilmStudioPresidentContactId = president.FilmStudioPresidentContactId,
        FilmStudioId = Guid.NewGuid().ToString(),

      };
      var user = new User
      {
        Username = registerModel.UserName,
        Password = registerModel.Password,
        UserId = Guid.NewGuid().ToString(),
        Role = Role.FilmStudio,
        FilmStudioId = studio.FilmStudioId
      };
      if (context.Users.Any(x => x.Username == registerModel.UserName))
      {
        throw new AppException("Username \"" + user.Username + "\" is already taken");
      }
      await context.FilmStudioPresidentContact.AddAsync(president);
      await context.FilmStudios.AddAsync(studio);
      await context.Users.AddAsync(user);
      context.SaveChanges();
      return userService.GetById(user.UserId);
    }

  }
}