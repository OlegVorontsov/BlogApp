namespace BlogApp.Server.Models
{
    public class UserShortModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[]? Photo { get; set; }
    }
}
