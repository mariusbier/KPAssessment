using UserServiceAPI.DataContext.Models;

namespace UserServiceAPI.ViewModels
{
    public class GroupUserResponse
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public GroupUserResponse(User user)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }
    }
}
