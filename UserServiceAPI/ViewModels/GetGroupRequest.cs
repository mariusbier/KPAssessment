namespace UserServiceAPI.ViewModels
{
    public class GetGroupRequest : DefaultRequest
    {
        public bool IncludeUsers { get; set; }
        public bool IncludePermissions { get; set; }
    }
}
