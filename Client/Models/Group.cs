namespace Client.Models
{
    public class Group
    {
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; } = new List<User>();
        public List<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
