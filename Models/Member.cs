namespace TennisApp2.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<Schedule>? Schedule { get; set; }
    }
}
