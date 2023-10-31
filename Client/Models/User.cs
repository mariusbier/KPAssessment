
namespace Client.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Group> Groups { get; set; } = new List<Group>();
    }
}
