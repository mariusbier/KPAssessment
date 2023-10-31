namespace Client.Models
{
    public class GetGroupRequest
    {
        public Guid GroupId { get; set; }
        public bool IncludeUsers { get; set; }
        public bool IncludePermissions { get; set; }
    }
}
