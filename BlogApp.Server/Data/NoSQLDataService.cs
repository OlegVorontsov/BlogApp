using LiteDB;

namespace BlogApp.Server.Data
{
    public class NoSQLDataService
    {
        private readonly string DBPath = "";
        private const string SubsCollection = "SubsCollection";
        private const string NewsLikesCollection = "NewsLikesCollection";

        public UserSub GetUserSub(int userId)
        {
            using (var db = new LiteDatabase(DBPath))
            {
                var subs = db.GetCollection<UserSub>(SubsCollection);
                var subForUser = subs.FindOne(x => x.UserId == userId);
                return subForUser;
            }
        }
        public UserSub SetUserSub(int from, int to)
        {
            using (var db = new LiteDatabase(DBPath))
            {
                var subs = db.GetCollection<UserSub>(SubsCollection);
                var subForUser = subs.FindOne(x => x.UserId == from);
                if (subForUser != null)
                {
                    if (!subForUser.UserIds.Contains(to))
                    {
                        subForUser.UserIds.Add(to);
                        subs.Update(subForUser);
                    }
                }
                else
                {
                    var newSubForUser = new UserSub
                    {
                        UserId = from,
                        UserIds = new List<int> { to }
                    };
                    subs.Insert(newSubForUser);
                    subs.EnsureIndex(x => x.UserId);

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
                        NewsId = from,
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
