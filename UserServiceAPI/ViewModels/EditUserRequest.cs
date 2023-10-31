namespace UserServiceAPI.ViewModels
{
    public class EditUserRequest : DefaultRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
