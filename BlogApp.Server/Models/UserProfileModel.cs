namespace BlogApp.Server.Models
{
    public class UserProfileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public byte[]? Photo { get; set; }
        public int SubsCount { get; set; }
    }
}
