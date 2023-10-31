namespace Client.Models
{
    public class AssignGroupsToUserRequest
    {
        public Guid UserId { get; set; }
        public List<Guid> GroupIds { get; set; }
    }
}
