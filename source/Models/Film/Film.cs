using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace filmstudion.api.Models
{
  public class Film : IFilm
  {
    public string FilmId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public DateTime ReleaseDate { get; set; }
    [Required]
    public string Country { get; set; }
    [Required]
    public string Director { get; set; }
    public List<FilmCopy> FilmCopies { get; set; }

  }
}