using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using filmstudion.api.Data;
using filmstudion.api.Helpers;
using filmstudion.api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace filmstudion.api.Services
{
  public interface IUserService
  {
    User Authenticate(string username, string password);
    IEnumerable<User> GetAll();
    User GetById(string id);
    User Create(User user, string password);
  }

  public class UserService : IUserService
  {
    private APIDbContext context;
    private readonly AppSettings _appSettings;

    public UserService(APIDbContext context, IOptions<AppSettings> appSettings)
    {
      this.context = context;
      _appSettings = appSettings.Value;
    }

    public User Authenticate(string username, string password)
    {
      var user = context.Users.SingleOrDefault(x => x.Username == username && x.Password == password);

      // return null if user not found
      if (user == null)
        return null;

      // authentication successful so generate jwt token
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
          {
                    new Claim(ClaimTypes.Name, user.UserId),
                    new Claim(ClaimTypes.Role, user.Role)
          }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      user.Token = tokenHandler.WriteToken(token);

      return user;
    }

    public IEnumerable<User> GetAll()
    {
      return context.Users.Include(u => u.filmStudio);
    }

    public User GetById(string id)
    {
      return context.Users.Include(u => u.filmStudio).ThenInclude(s => s.filmStudioPresidentContact).FirstOrDefault(u => u.UserId == id);
    }

    public User Create(User user, string password)
    {
      // validation
      if (string.IsNullOrWhiteSpace(password))
        throw new AppException("Password is required");

      if (context.Users.Any(x => x.Username == user.Username))
        throw new AppException("Username \"" + user.Username + "\" is already taken");

      context.Users.Add(user);
      context.SaveChanges();

      return user;
    }
  }
}