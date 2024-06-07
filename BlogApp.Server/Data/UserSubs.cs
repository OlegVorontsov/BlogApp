namespace BlogApp.Server.Data
{
    public class UserSubs
    {
        public int Id { get; set; }
        public List<UserSub> UserSubsList { get; set; }
    }
    public class UserSub
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
    }
}
