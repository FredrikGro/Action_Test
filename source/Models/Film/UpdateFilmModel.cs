using System;
using System.ComponentModel.DataAnnotations;

namespace filmstudion.api.Models
{
  public class UpdateFilmModel
  {
    public string FilmId{get;set;}
    [Required]
    public string Name { get; set; }
    [Required]
    public DateTime ReleaseDate { get; set; }
    [Required]
    public string Country { get; set; }
    [Required]
    public string Director { get; set; }
  }
}