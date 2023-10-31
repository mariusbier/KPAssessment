namespace UserServiceAPI.ViewModels
{
    public class AssignGroupsToUserRequest : DefaultRequest
    {
        public List<Guid> GroupIds { get; set; }
    }
}
