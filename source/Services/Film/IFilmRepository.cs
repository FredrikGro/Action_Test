using System.Collections.Generic;
using System.Threading.Tasks;

namespace filmstudion.api.Models
{
  public interface IFilmRepository
  {
    public IEnumerable<Film> GetAllFilms { get; }
    public IEnumerable<Film> GetAllFilmsAnonymous { get; }
    public Task<Film> GetFilmFromIdAsync(string Id);
    public Task<Film> GetFilmAnonymousFromIdAsync(string filmId);
    public Task<Film> CreateFilmAsync(CreateFilmModel createFilmModel);
    public Task<Film> UpdateFilmAsync(string Id, UpdateFilmModel updateFilmModel);
    public Task<Film> UpdateNoOfFilmCopies(string filmId, int noOfCopies);
    public Task<bool> NoOfFilmCopiesUpdateIsValid(string filmId, int noOfCopies);
    public Task<bool> HasAvailableCopyAsync(string FilmId);
    public Task<FilmCopy> RentCopyAsync(string filmId, string filmStudioId);
    public FilmCopy ReturnFilmCopyAsync(string filmStudioId, string filmId);
    public string GetFilmStudioIdFromIdentityName(string IdentityUserName);
  }
}