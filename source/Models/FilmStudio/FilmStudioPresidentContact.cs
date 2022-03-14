using Microsoft.EntityFrameworkCore;

namespace filmstudion.api.Models
{
  public class FilmStudioPresidentContact
  {
    public string FilmStudioPresidentContactId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
  }
}