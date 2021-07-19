using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication13.Configs;
using WebApplication13.Entities;
using WebApplication13.Entities.Data;

namespace WebApplication13.Services.UserServices
{
    public class UserService : IUserService
    {

        private readonly List<ViewModel.Vm_User> _users = new List<ViewModel.Vm_User>
        {
            new ViewModel.Vm_User
            {
                Id = 1, FirstName = "Alireza", LastName = "Shearchi", Username = "User1", Password = "1234",AccessAllUser = true
            },
            new ViewModel.Vm_User
            {
                Id = 2, FirstName = "hassan", LastName = "saeedi", Username = "User2", Password = "1234", AccessAllUser = false
            }
        };
        private readonly AppSettings _appSettings;
        private readonly TaskDbContext db;

        public UserService(TaskDbContext _taskDbContext, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            db = _taskDbContext;
        }

        public string AddUser(User user)
        {
            try
            {
                var result = db.Users.Add(user);
                return "Ok";

            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }

        public ViewModel.Vm_User Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var claims = new ClaimsIdentity();
            
                claims.AddClaims(new[]
                {
                    new Claim("AccessAllUser", user.AccessAllUser.ToString())
                });
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }

        public IEnumerable<ViewModel.Vm_User> GetAll()
        {
            return _users.ToList();
        }
    }
}
