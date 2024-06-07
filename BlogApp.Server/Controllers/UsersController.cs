using BlogApp.Server.Data;
using BlogApp.Server.Models;
using BlogApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _userService;
        public UsersController(UsersService userService)
        {
            _userService = userService;
        }
        [HttpGet("all/{name}")]
        public IActionResult GetUsersByName(string name)
        {
            return Ok(_userService.GetUsersByName(name));
        }
        [HttpPost("subs/{userId}")]
        public IActionResult Subscribe(int userId)
        {
            var currentUser = _userService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }
            if (currentUser.Id != userId) _userService.Subscribe(from: currentUser.Id, to: userId);
            else return BadRequest();
            return Ok();
        }
        [HttpGet("{userId}")]
        public IActionResult Get(int userId)
        {
            return Ok(_userService.GetUserProfileModelById(userId));
        }
        [HttpPost("create")]
        public IActionResult CreateUsers([FromBody] List<UserModel> users)
        {
            var currentUser = _userService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }
            if (currentUser.Id != 1) return BadRequest();
            var listEserModels = _userService.CreateUsers(users);
            return Ok(listEserModels);
        }
    }
}
