using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace filmstudion.api.Models
{

  interface IFIilmStudio
  {
    public string FilmStudioId { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string FilmStudioPresidentContactId { get; set; }
    public FilmStudioPresidentContact filmStudioPresidentContact { get; set; }
    public List<FilmCopy> RentedFilmCopies { get; set; }
  }
}