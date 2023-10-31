using UserServiceAPI.DataContext.Models;

namespace UserServiceAPI.ViewModels
{
    public class GetPermissionResponse : DefaultRequest
    {
        public string Name { get; set; }
        public List<GetGroupResponse> Groups { get; set; }

        public GetPermissionResponse(Permission permission)
        {
            PermissionId = permission.PermissionId;
            Name = permission.Name;
            Groups = permission.Groups != null ? permission.Groups.Select(s => new GetGroupResponse(s)).ToList() : new List<GetGroupResponse>();
        }
    }
}
