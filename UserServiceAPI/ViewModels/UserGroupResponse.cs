using UserServiceAPI.DataContext.Models;

namespace UserServiceAPI.ViewModels
{
    public class UserGroupResponse
    {
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public List<GroupPermissionResponse> Permissions { get; set; }

        public UserGroupResponse(Group group)
        {
            GroupId = group.GroupId;
            Name = group.Name;
            Permissions = group.Permissions != null ? group.Permissions.Select(s => new GroupPermissionResponse(s)).ToList() : new List<GroupPermissionResponse>();
        }
    }
}
