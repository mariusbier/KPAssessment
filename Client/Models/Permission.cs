namespace Client.Models
{
    public class Permission
    {
        public Guid PermissionId { get; set; }
        public string Name { get; set; }
        public List<Group> Groups { get; set; } = new List<Group>();
    }
}
