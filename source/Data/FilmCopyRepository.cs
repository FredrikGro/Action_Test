using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using filmstudion.api.Models;

namespace filmstudion.api.Data
{
  public class FilmCopyRepository : IFilmCopyRepository
  {
    private readonly APIDbContext context;

    public FilmCopyRepository(APIDbContext context)
    {
      this.context = context;
    }
    public Task<IEnumerable<FilmCopy>> GetAllFilmCopies => throw new System.NotImplementedException();

    public void CreateFilmCopy(string filmId)
    {
      context.FilmCopies.Add(new FilmCopy { FilmId = filmId, RentedOut = false, FilmCopyId = Guid.NewGuid().ToString() });
      context.SaveChanges();
      return;
    }

    public bool DeleteFilmCopyFromId(string FilmCopyId)
    {
      try
      {
        context.FilmCopies.Remove(GetFilmCopyFromId(FilmCopyId));
        context.SaveChanges();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public FilmCopy GetFilmCopyFromId(string Id)
    {
      return context.FilmCopies.FirstOrDefault(c => c.FilmCopyId == Id);
    }
  }
}