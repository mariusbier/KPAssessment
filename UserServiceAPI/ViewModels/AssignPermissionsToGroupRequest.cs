namespace UserServiceAPI.ViewModels
{
    public class AssignPermissionsToGroupRequest : DefaultRequest
    {
        public List<Guid> PermissionIds { get; set; }
    }
}
