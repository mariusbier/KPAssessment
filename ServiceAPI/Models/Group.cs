namespace ServiceAPI.Models
{
    public class Group
    {
        public Guid GroupId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public List<User> Users { get; set; }
        public virtual List<Permission> Permissions { get; set; }
    }
}
