using System.ComponentModel.DataAnnotations;

namespace filmstudion.api.Models
{
  public class UserRegisterModel
  {
    [Required]
    public bool IsAdmin { get; set; }
    public string FilmStudioId { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
  }

}