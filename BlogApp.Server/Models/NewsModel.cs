namespace BlogApp.Server.Models
{
    public class NewsModel
    {
        public int Id { get; set; }
        public string Text { get; private set; }
        public byte[] Img { get; private set; }
        public int LikesCount { get; set; }
        public DateTime PostDate { get; private set; }
        public NewsModel(int id, string text, byte[] img, DateTime postDate)  
        {
            Id = id;
            Text = text;
            Img = img;
            PostDate = postDate;
        }
    }
}
