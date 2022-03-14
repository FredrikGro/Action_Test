using System.Collections.Generic;
using System.Threading.Tasks;

namespace filmstudion.api.Models
{
  public interface IFilmCopyRepository
  {
    public Task<IEnumerable<FilmCopy>> GetAllFilmCopies { get; }
    public FilmCopy GetFilmCopyFromId(string Id);
    public bool DeleteFilmCopyFromId(string FilmCopyId);
    public void CreateFilmCopy(string filmId);
  }
}