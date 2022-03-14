using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using filmstudion.api.Data;
using filmstudion.api.Helpers;
using filmstudion.api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace filmstudion.api.Controllers

{
  [Authorize]
  [ApiController]
  [Route("[controller]")]

  public class FilmStudiosController : ControllerBase
  {
    private readonly IFilmStudioRepository filmStudioRepository;
    private readonly APIDbContext context;
    private readonly IFilmRepository filmRepository;

    public FilmStudiosController(IFilmStudioRepository filmStudioRepository, APIDbContext context, IFilmRepository filmRepository)
    {
      this.filmStudioRepository = filmStudioRepository;
      this.context = context;
      this.filmRepository = filmRepository;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<FilmStudioRegisterModel>> RegisterFilmStudio([FromBody] FilmStudioRegisterModel registerModel)
    {
      try
      {
        var user = await filmStudioRepository.RegisterFilmStudioAsync(registerModel);
        return Ok(user);
      }
      catch (AppException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
    }

    [Authorize(Roles = Role.FilmStudio)]
    [Route("/[controller]/{FilmStudioId}/RentFilm/{FilmId}")]
    [HttpPost]
    public async Task<ActionResult<FilmCopy>> RentFilm(string FilmStudioId, string FilmId)
    {
      string IdentityUserId = User.Identity.Name.ToString();
      string LoggedInStudioId = filmRepository.GetFilmStudioIdFromIdentityName(IdentityUserId);
      if (String.IsNullOrEmpty(LoggedInStudioId))
      {
        return StatusCode(500, "Logged in studio not found");
      }
      if (!filmStudioRepository.FilmStudioExists(FilmStudioId))
      {
        return StatusCode(400, "Requested filmstudio Id doesn't exist.");
      }
      if (await filmRepository.GetFilmFromIdAsync(FilmId) == null)
      {
        return StatusCode(400, "No film with this Id exists.");
      }
      if (LoggedInStudioId != FilmStudioId)
      {
        return StatusCode(403, "Cannot return film for film studio which is not authenticated.");
      }
      if (filmStudioRepository.FilmStudioHasOngoingRentOfCopy(FilmStudioId, FilmId))
      {
        return StatusCode(403, "This film studio is currently already renting a copy of this movie.");
      }
      if (await filmRepository.HasAvailableCopyAsync(FilmId) == false)
      {
        return StatusCode(403, "No copies of this film are currently available.");
      }
      try
      {
        var result = await filmRepository.RentCopyAsync(FilmStudioId, FilmId);
        return Ok(result);
      }
      catch
      {
        return StatusCode(500, "Something went wrong on the server...");
      }
    }

    [Authorize(Roles = Role.FilmStudio)]
    [Route("/[controller]/{FilmStudioId}/ReturnFilm/{FilmId}")]
    [HttpPost]
    public async Task<ActionResult<FilmCopy>> ReturnFilm(string FilmStudioId, string FilmId)
    {
      string IdentityUserId = User.Identity.Name.ToString();
      string LoggedInStudioId = filmRepository.GetFilmStudioIdFromIdentityName(IdentityUserId);
      if (String.IsNullOrEmpty(LoggedInStudioId))
      {
        return StatusCode(500, "Logged in studio not found");
      }
      if (!filmStudioRepository.FilmStudioExists(FilmStudioId))
      {
        return StatusCode(400, "Requested filmstudio Id doesn't exist.");
      }
      if (await filmRepository.GetFilmFromIdAsync(FilmId) == null)
      {
        return StatusCode(400, "No film with this Id exists.");
      }
      if (LoggedInStudioId != FilmStudioId)
      {
        return StatusCode(403, "Cannot return film for film studio which is not authenticated.");
      }
      if (!filmStudioRepository.FilmStudioHasOngoingRentOfCopy(FilmStudioId, FilmId))
      {
        return StatusCode(403, "This film studio currently doesn't rent a copy of this movie.");
      }
      try
      {
        var result = filmRepository.ReturnFilmCopyAsync(FilmStudioId, FilmId);
        return Ok(result);
      }
      catch
      {
        return StatusCode(500, "Something went wrong on the server...");
      }
    }

    [AllowAnonymous]
    [HttpGet]
    public ActionResult<IEnumerable<FilmStudio>> GetAllFilmStudios()
    {
      if (User.IsInRole(Role.Admin))
      {
        var studios = filmStudioRepository.GetAllFilmStudios;
        if (studios != null)
        {
          return Ok(studios);
        }
        else
        {
          return NotFound();
        }
      }

      var studiosAnonymous = filmStudioRepository.GetAllFilmStudiosAnonymous;
      if (studiosAnonymous != null)
      {
        return Ok(studiosAnonymous);
      }
      else
      {
        return NotFound();
      }
    }

    [AllowAnonymous]
    [Route("/[controller]/{FilmStudioId}")]
    [HttpGet]
    public ActionResult<IEnumerable<FilmStudio>> GetFilmStudio(string FilmStudioId)
    {
      if (!filmStudioRepository.FilmStudioExists(FilmStudioId))
      {
        return StatusCode(400, "Requested filmstudio Id doesn't exist.");
      }

      if (User.IsInRole(Role.Admin))
      {
        var studio = filmStudioRepository.GetFilmStudioFromId(FilmStudioId);
        if (studio != null)
        {
          return Ok(studio);
        }
        else
        {
          return NotFound();
        }
      }

      if (User.IsInRole(Role.FilmStudio))
      {
        string IdentityUserId = User.Identity.Name.ToString();
        string LoggedInStudioId = filmRepository.GetFilmStudioIdFromIdentityName(IdentityUserId);
        if (LoggedInStudioId == FilmStudioId)
        {
          var studio = filmStudioRepository.GetFilmStudioFromId(FilmStudioId);
          if (studio != null)
          {
            return Ok(studio);
          }
          else
          {
            return NotFound();
          }
        }
      }

      var studioAnonymous = filmStudioRepository.GetFilmStudioFromIdAnonymous(FilmStudioId);
      if (studioAnonymous != null)
      {
        return Ok(studioAnonymous);
      }
      else
      {
        return NotFound();
      }

    }
  }
}
