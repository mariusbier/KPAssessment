namespace Client.Models
{
    public class AddUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Guid> GroupIds { get; set; }
    }
}
