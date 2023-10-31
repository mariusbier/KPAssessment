using UserServiceAPI.DataContext.Models;

namespace UserServiceAPI.ViewModels
{
    public class GetGroupResponse
    {
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public List<GroupUserResponse> Users { get; set; }
        public List<GroupPermissionResponse> Permissions { get; set; }

        public GetGroupResponse(Group group)
        {
            GroupId = group.GroupId;
            Name = group.Name;
            Users = group.Users != null ? group.Users.Select(s => new GroupUserResponse(s)).ToList() : new List<GroupUserResponse>();
            Permissions = group.Permissions != null ? group.Permissions.Select(s => new GroupPermissionResponse(s)).ToList() : new List<GroupPermissionResponse>();
        }
    }
}
