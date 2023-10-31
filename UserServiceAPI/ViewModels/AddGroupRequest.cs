namespace UserServiceAPI.ViewModels
{
    public class AddGroupRequest
    {
        public string Name { get; set; }
        public List<Guid>? PermissionIds { get; set; }
    }
}
