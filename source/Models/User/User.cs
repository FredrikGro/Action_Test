using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace filmstudion.api.Models
{
  public class User
  {
    [Required]
    public string UserId { get; set; }
    [Required]
    public string Role { get; set; }
    [Required]
    public string Username { get; set; }
    public string FilmStudioId { get; set; }
    public FilmStudio filmStudio { get; set; }
    [Required]
    public string Password { get; set; }
    public string Token { get; set; }

  }
}