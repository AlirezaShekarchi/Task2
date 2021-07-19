using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using WebApplication13.DtoModels;
using WebApplication13.Entities;
using WebApplication13.Services.UserServices;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication13.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _userService = userService;
            _logger = logger;
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public ViewModel.Vm_User Login([FromBody] UserDto user)
        {
            _logger.LogInformation("Login");
            var User = _userService.Authenticate(user.Username, user.Password);

            if (User == null)
            {
                return null;
            }
            return User;
        }
        [Authorize(Policy = "GetAllUser")]
        [HttpPost("AddUser")]
        public string AddUser([FromBody] User user)
        {
            _logger.LogInformation("AddUser");
            var result = _userService.AddUser(user);
            return result;
        }
        [Authorize(Policy ="GetAllUser")]
        [HttpGet("GetAllUser")]
        public IEnumerable<ViewModel.Vm_User> GetAllUser()
        {
            return _userService.GetAll();
        }
    }
}
