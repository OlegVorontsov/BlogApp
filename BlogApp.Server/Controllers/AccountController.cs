using BlogApp.Server.Data;
using BlogApp.Server.Models;
using BlogApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlogApp.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UsersService _userService;
        public AccountController (UsersService usersService)
        {
            _userService = usersService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var currentUserEmail = HttpContext.User.Identity.Name;
            var currentUser = _userService.GetUserByLogin(currentUserEmail);
            if (currentUser is null )
            {
                return NotFound();
            }
            return Ok( new UserModel
            {
                Id = currentUser.Id,
                Name = currentUser.Name,
                Email = currentUser.Email,
                Description = currentUser.Description,
                Photo = currentUser.Photo
            });
        }
        [HttpPost]
        [AllowAnonymous]
        public object Create([FromBody] UserModel user)
        {
            var newUser = _userService.Create(user);
            return Ok(newUser);
        }
        [HttpPatch] //методы изменения данных
        public object Update(UserModel user)
        {
            var currentUserEmail = HttpContext.User.Identity.Name;
            var currentUser = _userService.GetUserByLogin(currentUserEmail);
            if (currentUser!=null && currentUser.Id != user.Id)
            {
                return BadRequest();
            }
            _userService.Update(currentUser, user);
            return Ok(user);
        }
        [HttpDelete]
        public IActionResult Delete()
        {
            var currentUserEmail = HttpContext.User.Identity.Name;
            var currentUser = _userService.GetUserByLogin(currentUserEmail);
            if (currentUser is null)
            {
                return NotFound();
            }
            _userService.DeleteUser(currentUser);
            return Ok();
        }
        [HttpPost("token")]
        [AllowAnonymous]
        public object GetToken()
        {
            var userData = _userService.GetUserLoginPassFromBasicAuth(Request);
            (ClaimsIdentity claims, int id)? identity = _userService.GetIdentity(userData.login, userData.password);
            if (identity is null)
            {
                return NotFound("login or password is not correct");
            }
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity?.claims.Claims,
                    expires: now.AddMinutes(AuthOptions.LIFETIME),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var tokenModel = new AuthToken(
                minutes: AuthOptions.LIFETIME,
                accessToken: encodedJwt,
                userName: userData.login,
                userId: identity.Value.id
                );

            return Ok(tokenModel);
        }
    }
}
