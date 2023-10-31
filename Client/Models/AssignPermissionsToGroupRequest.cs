namespace Client.Models
{
    public class AssignPermissionsToGroupRequest
    {
        public Guid GroupId { get; set; }
        public List<Guid> PermissionIds { get; set; }
    }
}
