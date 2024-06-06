namespace BlogApp.Server.Data
{
    public class News
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Text { get; set; }
        public byte[]? Img { get; set; }
        public DateTime PostDate { get; set; }
    }
}
