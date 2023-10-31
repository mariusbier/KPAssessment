namespace ServiceAPI.Models
{
    public class User
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<Group> Groups { get; set; }
    }
}
