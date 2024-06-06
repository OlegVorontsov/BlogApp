using BlogApp.Server.Data;
using BlogApp.Server.Models;

namespace BlogApp.Server.Services
{
    public class NewsService
    {
        private readonly DataContext _dataContext;
        private readonly NoSQLDataService _noSQLDataService;
        public NewsService(DataContext dataContext, NoSQLDataService noSQLDataService)
        {
            _dataContext = dataContext;
            _noSQLDataService = noSQLDataService;
        }
        public List<NewsModel> GetByAuthor(int userId)
        {
            var news = _dataContext.News.Where(n => n.AuthorId == userId)
                                        .Reverse()
                                        .Select(ToModel)
                                        .ToList();
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
            var newsToUpdate = _dataContext.News
                                           .FirstOrDefault(n => n.Id == newsModel.Id && n.AuthorId == userId);
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
            var newsToDelete = _dataContext.News
                                           .FirstOrDefault(n => n.Id == newsId && n.AuthorId == userId);
            if (newsToDelete == null)
            {
                return;
            }
            _dataContext.News.Remove(newsToDelete);
            _dataContext.SaveChangesAsync();
        }
        public List<NewsModel> GetNewsForCurrentUser(int userId)
        {
            var subs = _noSQLDataService.GetUserSub(userId);
            var allNews = new List<NewsModel>();
            foreach (var sub in subs)
            {
                var allNewsByAuthor = _dataContext.News
                                                  .Where(n => n.AuthorId == sub.To).ToList();
                allNews.AddRange(allNewsByAuthor.Select(ToModel));
            }
            allNews.Sort(new NewsComparer());
            return allNews;
        }
        public void SetLike(int newsId, int userId)
        {
            _noSQLDataService.SetNewsLike(
                from: userId,
                newsId: newsId);
        }
        private NewsModel ToModel(News news)
        {
            var likes = _noSQLDataService.GetNewsLike(news.Id);
            var newsModel = new NewsModel
                (
                    id: news.Id,
                    text: news.Text,
                    img: news.Img,
                    postDate: news.PostDate
                );
            newsModel.LikesCount = likes.UserIds.Count;
            return newsModel;
        }
    }
    class NewsComparer : IComparer<NewsModel>
    {
        public int Compare(NewsModel? x, NewsModel? y)
        {
            if(x.PostDate > y.PostDate) return -1;
            if (x.PostDate < y.PostDate) return 1;
            return 0;
        }
    }
}
