namespace ServiceAPI.Models
{
    public class Permission
    {
        public Guid PermissionId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
    }
}
