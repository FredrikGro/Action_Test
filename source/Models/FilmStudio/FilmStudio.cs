using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace filmstudion.api.Models
{
  public class FilmStudio
  {
    public string FilmStudioId { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string FilmStudioPresidentContactId { get; set; }
    public FilmStudioPresidentContact filmStudioPresidentContact { get; set; }
    public List<FilmCopy> RentedFilmCopies { get; set; }
  }
}