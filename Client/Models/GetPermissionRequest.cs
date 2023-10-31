namespace Client.Models
{
    public class GetPermissionRequest
    {
        public Guid PermissionId { get; set; }
        public bool IncludeGroups { get; set; }
    }
}
