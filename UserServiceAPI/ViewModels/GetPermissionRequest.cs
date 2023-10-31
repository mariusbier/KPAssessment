namespace UserServiceAPI.ViewModels
{
    public class GetPermissionRequest : DefaultRequest
    {
        public bool IncludeGroups { get; set; }
    }
}
