using BlogApp.Server.Data;
using BlogApp.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Server.Services
{
    public class NewsService
    {
        private readonly DataContext _dataContext;
        public NewsService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public List<News> GetByAuthor(int userId)
        {
            var news = _dataContext.News.Where(n => n.AuthorId == userId).Reverse().ToList();
            return news;
        }
        public NewsModel Create (NewsModel newsModel, int userId)
        {
            var newNews = new News
            {
                AuthorId = userId,
                Text = newsModel.Text,
                Img = newsModel.Img,
                PostDate = DateTime.UtcNow
            };
            _dataContext.News.Add(newNews);
            _dataContext.SaveChanges();

            newsModel.Id = newNews.Id;
            return newsModel;
        }
        public NewsModel Update(NewsModel newsModel, int userId)
        {
            var newsToUpdate = _dataContext.News.FirstOrDefault(n => n.Id == newsModel.Id && n.AuthorId == userId);
            if (newsToUpdate == null)
            {
                return null;
            }
            newsToUpdate.Text = newsModel.Text;
            newsToUpdate.Img = newsModel.Img;

            _dataContext.News.Update(newsToUpdate);
            _dataContext.SaveChanges();

            return newsModel;
        }
        public void Delete(int newsId, int userId)
        {
            var newsToDelete = _dataContext.News.FirstOrDefault(n => n.Id == newsId && n.AuthorId == userId);
            if (newsToDelete == null)
            {
                return;
            }
            _dataContext.News.Remove(newsToDelete);
            _dataContext.SaveChangesAsync();
        }
    }
}
