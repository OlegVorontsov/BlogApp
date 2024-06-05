using BlogApp.Server.Models;
using BlogApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class NewsController : ControllerBase
    {
        private readonly NewsService _newsService;
        public NewsController(NewsService newsService)
        {
            _newsService = newsService;
        }
        [HttpGet("{userId}")]
        public IActionResult GetByAuthor(int userId)
        {
            var news = _newsService.GetByAuthor(userId);
            return Ok(news);
        }
        [HttpPost]
        public IActionResult Create([FromBody] NewsModel newsModel)
        {

        }
    }
}
