using LiteDB;
using System.Reflection;

namespace BlogApp.Server.Data
{
    public class NoSQLDataService
    {
        private readonly string DBPath = "BlogApp_NoSQLDB.db";

        private const string SubsCollection = "SubsCollection";
        private const string NewsLikesCollection = "NewsLikesCollection";

        public UserSubs GetUserSub(int userId)
        {
            using (var db = new LiteDatabase(DBPath))
            {
                var subs = db.GetCollection<UserSubs>(SubsCollection);
                var subForUser = subs.FindOne(x => x.Id == userId);
                return subForUser;
            }
        }
        public UserSubs SetUserSub(int from, int to)
        {
            using (var db = new LiteDatabase(DBPath))
            {
                var subs = db.GetCollection<UserSubs>(SubsCollection);
                var subForUser = subs.FindOne(x => x.Id == from);
                var sub = new UserSub
                {
                    Id = to,
                    Date = DateTime.Now
                };
                if (subForUser != null)
                {
                    if (!subForUser.UserSubsList.Select(x => x.Id).Contains(to))
                    {
                        subForUser.UserSubsList.Add(sub);
                        subs.Update(subForUser);
                    }
                }
                else
                {
                    var newSubForUser = new UserSubs
                    {
                        Id = from,
                        UserSubsList = new List<UserSub> { sub }
                    };
                    subs.Insert(newSubForUser);
                    subs.EnsureIndex(x => x.Id);

                    subForUser = newSubForUser;
                }
                return subForUser;
            }
        }
        public NewsLike GetNewsLike(int newsId)
        {
            using (var db = new LiteDatabase(DBPath))
            {
                var likes = db.GetCollection<NewsLike>(NewsLikesCollection);
                var newsLike = likes.FindOne(x => x.NewsId == newsId);
                return newsLike;
            }
        }
        public NewsLike SetNewsLike(int from, int newsId)
        {
            using (var db = new LiteDatabase(DBPath))
            {
                var likes = db.GetCollection<NewsLike>(NewsLikesCollection);
                var newsLikes = likes.FindOne(x => x.NewsId == newsId);
                if (newsLikes != null)
                {
                    if (!newsLikes.UserIds.Contains(from))
                    {
                        newsLikes.UserIds.Add(from);
                        likes.Update(newsLikes);
                    }
                }
                else
                {
                    var newLikeForNews = new NewsLike
                    {
                        NewsId = newsId,
                        UserIds = new List<int> { from }
                    };
                    likes.Insert(newLikeForNews);
                    likes.EnsureIndex(x => x.NewsId);

                    newsLikes = newLikeForNews;
                }
                return newsLikes;
            }
        }
    }
}
