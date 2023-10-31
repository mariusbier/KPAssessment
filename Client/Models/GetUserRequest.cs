namespace Client.Models
{
    public class GetUserRequest
    {
        public Guid UserId { get; set; }
        public bool IncludeGroups { get; set; }
    }
}
