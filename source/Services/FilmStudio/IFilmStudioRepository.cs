using System.Collections.Generic;
using System.Threading.Tasks;

namespace filmstudion.api.Models
{
  public interface IFilmStudioRepository
  {
    public IEnumerable<FilmStudio> GetAllFilmStudios { get; }
    public IEnumerable<FilmStudio> GetAllFilmStudiosAnonymous { get; }
    public Task<User> RegisterFilmStudioAsync(FilmStudioRegisterModel studio);
    public bool FilmStudioExists(string FilmStudioId);
    public bool FilmStudioHasOngoingRentOfCopy(string filmStudioId, string filmId);
    public FilmStudio GetFilmStudioFromId(string filmStudioId);
    public FilmStudio GetFilmStudioFromIdAnonymous(string requestedFilmStudioId);
  }
}