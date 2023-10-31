using UserServiceAPI.DataContext.Models;

namespace UserServiceAPI.ViewModels
{
    public class GroupPermissionResponse
    {
        public Guid PermissionId { get; set; }
        public string Name { get; set; }

        public GroupPermissionResponse(Permission permission)
        {
            PermissionId = permission.PermissionId;
            Name = permission.Name;
        }
    }
}
