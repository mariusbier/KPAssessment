namespace UserServiceAPI.ViewModels
{
    public class GetUserRequest : DefaultRequest
    {
        public bool IncludeGroups { get; set; }
    }
}
