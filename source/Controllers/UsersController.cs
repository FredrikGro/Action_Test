using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using filmstudion.api.Helpers;
using filmstudion.api.Models;
using filmstudion.api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace filmstudion.api.Controllers
{
  [Authorize]
  [ApiController]
  [Route("[controller]")]
  public class UsersController : ControllerBase
  {
    private IUserService userService;
    private readonly AppSettings appSettings;

    public UsersController(IUserService userService, IOptions<AppSettings> appSettings)
    {
      this.userService = userService;
      this.appSettings = appSettings.Value;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register([FromBody] UserRegisterModel model)
    {
      var user = new User
      {
        Username = model.Username,
        Password = model.Password,
        Role = model.IsAdmin ? Role.Admin : Role.FilmStudio,
        FilmStudioId = model.IsAdmin ? null : model.FilmStudioId,
        UserId = Guid.NewGuid().ToString(),
      };

      try
      {
        userService.Create(user, model.Password);
        return Ok(user);
      }
      catch (AppException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult Authenticate([FromBody] AuthenticateModel model)
    {
      var user = userService.Authenticate(model.Username, model.Password);

      if (user == null)
        return BadRequest(new { message = "Username or password is incorrect" });

      return Ok(user);
    }

    [Authorize(Roles = Role.Admin)]
    [HttpGet]
    public IActionResult GetAll()
    {
      var users = userService.GetAll();
      return Ok(users);
    }
  }
}