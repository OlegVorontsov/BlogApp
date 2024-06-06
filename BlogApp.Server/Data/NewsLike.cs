namespace BlogApp.Server.Data
{
    public class NewsLike
    {
        public int NewsId { get; set; }
        public List<int> UserIds { get; set; }
    }
}
