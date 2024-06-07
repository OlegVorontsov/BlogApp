using BlogApp.Server.Models;
using BlogApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class NewsController : ControllerBase
    {
        private readonly UsersService _userService;
        private readonly NewsService _newsService;
        public NewsController(UsersService userService, NewsService newsService)
        {
            _userService = userService;
            _newsService = newsService;
        }
        [HttpGet("{userId}")]
        public IActionResult GetByAuthor(int userId)
        {
            var news = _newsService.GetByAuthor(userId);
            return Ok(news);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var currentUser = _userService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }
            var news = _newsService.GetNewsForCurrentUser(currentUser.Id);
            return Ok(news);
        }
        [HttpPost]
        public IActionResult Create([FromBody] NewsModel newsModel)
        {
            var currentUser = _userService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }
            var newsModelNew = _newsService.Create(newsModel, currentUser.Id);
            return Ok(newsModelNew);
        }
        [HttpPost("all")]
        public IActionResult Create([FromBody] List<NewsModel> newsModels)
        {
            var currentUser = _userService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }
            var newsModelsNew = _newsService.CreateNews(newsModels, currentUser.Id);
            return Ok(newsModelsNew);
        }
        [HttpPatch]
        public IActionResult Update([FromBody] NewsModel newsModel)
        {
            var currentUser = _userService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }
            var newsModelToUpdate = _newsService.Update(newsModel, currentUser.Id);
            return Ok(newsModelToUpdate);
        }
        [HttpDelete("{newsId}")]
        public IActionResult Delete(int newsId)
        {
            var currentUser = _userService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }
            _newsService.Delete(newsId, currentUser.Id);
            return Ok();
        }
        [HttpPost("Like/{newsId}")]
        public IActionResult SetLike(int newsId)
        {
            var currentUser = _userService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }
            _newsService.SetLike(newsId, currentUser.Id);
            return Ok();
        }
    }
}
