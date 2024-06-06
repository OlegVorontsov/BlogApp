using BlogApp.Server.Data;
using BlogApp.Server.Models;

namespace BlogApp.Server.Services
{
    public class NewsService
    {
        private readonly DataContext _dataContext;
        public NewsService(DataContext dataContext)
        {
            _dataContext = dataContext;
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
            var subs = _dataContext.UserSubs
                                   .Where(x => x.From == userId).ToList();
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
            var like = new NewsLike
            {
                From = userId,
                NewsId = newsId
            };
            _dataContext.NewsLikes.Add(like);
            _dataContext.SaveChangesAsync();
        }
        private NewsModel ToModel(News news)
        {
            var likes = _dataContext.NewsLikes.Where(x => x.NewsId == news.Id).Count();
            var newsModel = new NewsModel
                (
                    id: news.Id,
                    text: news.Text,
                    img: news.Img,
                    postDate: news.PostDate
                );
            newsModel.LikesCount = likes;
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
