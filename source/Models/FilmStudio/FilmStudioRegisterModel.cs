using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace filmstudion.api.Models
{
  public class FilmStudioRegisterModel
  {
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string FilmStudioName { get; set; }
    [Required]
    public string FilmStudioCity { get; set; }
    [Required]
    public string FilmStudioPresidentName { get; set; }
    [Required]
    public string FilmStudioPresidentAddress { get; set; }
    [Required]
    public string FilmStudioPresidentPhoneNumber { get; set; }
  }
}