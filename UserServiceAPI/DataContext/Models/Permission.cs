namespace UserServiceAPI.DataContext.Models
{
    public class Permission
    {
        public Guid PermissionId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public virtual List<Group> Groups { get; set; }
    }
}
