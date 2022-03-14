using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using filmstudion.api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace filmstudion.api.Controllers
{
  [Authorize]
  [ApiController]
  [Route("[controller]")]
  public class FilmsController : ControllerBase
  {
    private readonly IFilmRepository filmRepository;

    public FilmsController(IFilmRepository filmRepository)
    {
      this.filmRepository = filmRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult GetAllFilms()
    {
      if (User.IsInRole(Role.FilmStudio) || User.IsInRole(Role.Admin))
      {
        var films = filmRepository.GetAllFilms;
        if (films != null)
        {
          return Ok(films);
        }
        return NotFound();
      }
      else
      {
        var films = filmRepository.GetAllFilmsAnonymous;
        if (films != null)
        {
          return Ok(films);
        }
        return NotFound();
      }
    }

    [AllowAnonymous]
    [Route("/[Controller]/{FilmId}")]
    [HttpGet]
    public async Task<ActionResult<Film>> GetFilm(string FilmId)
    {
      if (User.IsInRole(Role.FilmStudio) || User.IsInRole(Role.Admin))
      {
        var film = await filmRepository.GetFilmFromIdAsync(FilmId);
        if (film != null)
        {
          return Ok(film);
        }
        return NotFound();
      }
      else
      {
        var film = await filmRepository.GetFilmAnonymousFromIdAsync(FilmId);
        if (film != null)
        {
          return Ok(film);
        }
        return NotFound();
      }
    }

    [Authorize(Roles = Role.Admin)]
    [HttpPost("create")]
    public async Task<ActionResult<Film>> CreateFilm([FromBody] CreateFilmModel createFilmModel)
    {
      var result = await filmRepository.CreateFilmAsync(createFilmModel);
      return Ok(result);
    }

    [Authorize(Roles = Role.Admin)]
    [Route("/[controller]/{FilmId}")]
    [HttpPut]
    public async Task<ActionResult<Film>> UpdateFilm(string FilmId, [FromBody] UpdateFilmModel updateFilmModel)
    {
      try
      {
        var result = await filmRepository.UpdateFilmAsync(FilmId, updateFilmModel);
        return Ok(result);
      }
      catch
      {
        return StatusCode(500, $"Could not update film with id: {FilmId}");
      }
    }

    [Authorize(Roles = Role.Admin)]
    [Route("/[controller]/{FilmId}/UpdateNoOfFilmCopies/{NoOfCopies=int}")]
    [HttpPut]
    public async Task<ActionResult<Film>> UpdateNoOfFilmCopies(string FilmId, int NoOfCopies)
    {
      if (NoOfCopies <= 0)
      {
        return StatusCode(406, "No of copies cannot be zero or negative.");
      }

      if (await filmRepository.NoOfFilmCopiesUpdateIsValid(FilmId, NoOfCopies) == false)
      {
        return StatusCode(406, "You are trying to erase film copies that are currently rented out. Try again when films have been restituted.");
      }
      try
      {
        var result = await filmRepository.UpdateNoOfFilmCopies(FilmId, NoOfCopies);
        return Ok(result);
      }
      catch
      {
        return StatusCode(500, $"Could not update number of film copies with id: {FilmId}");
      }
    }

  }
}

