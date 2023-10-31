using UserServiceAPI.DataContext.Models;

namespace UserServiceAPI.ViewModels
{
    public class GetUserResponse
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<UserGroupResponse> Groups { get; set; }

        public GetUserResponse(User user)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Groups = user.Groups != null ? user.Groups.Select(s => new UserGroupResponse(s)).ToList() : new List<UserGroupResponse>();
        }
    }
}
