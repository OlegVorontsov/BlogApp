namespace BlogApp.Server.Models
{
    public class NewsModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public byte[]? Img { get; set; }
        public int? LikesCount { get; set; }
        public DateTime PostDate { get; set; }
    }
}
