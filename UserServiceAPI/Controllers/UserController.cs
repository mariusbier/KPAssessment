using Microsoft.AspNetCore.Mvc;
using UserServiceAPI.DataContext;
using UserServiceAPI.Logic;
using UserServiceAPI.ViewModels;

namespace UserServiceAPI.Controllers
{
    [ApiController]
    [Route("/[controller]/[action]")]
    [Produces("application/json"), Consumes("application/json")]
    public class UserController : ControllerBase
    {
        private UserLogic _UserLogic;

        public UserController(
            DatabaseContext databaseContext)
            : base()
        {
            _UserLogic = new UserLogic(databaseContext);
        }

        [HttpPost]
        public GetUserResponse GetUser([FromBody] GetUserRequest userRequest)
        {
            if (userRequest != null && userRequest.UserId != new Guid())
            {
                var user = new GetUserResponse(_UserLogic.GetUser(userRequest.UserId, userRequest.IncludeGroups));
                return user;
            }
            return null;
        }

        [HttpGet]
        public List<GetUserResponse> GetAllUsers()
        {
            return _UserLogic.GetAllUsers().Select(s => new GetUserResponse(s)).ToList();
        }

        [HttpGet]
        public int GetUserCount()
        {
            return _UserLogic.CountAllUsers();
        }

        [HttpGet]
        public int GetUserGroupCount(Guid userId)
        {
            return _UserLogic.CountUserGroups(userId);
        }

        [HttpPost]
        public List<GetGroupResponse> GetUserGroups([FromBody] DefaultRequest userRequest)
        {
            if (userRequest != null && userRequest.UserId != new Guid())
            {
                var groups = _UserLogic.GetUserGroups(userRequest.UserId).Select(s => new GetGroupResponse(s)).ToList();
                return groups;
            }
            return null;
        }

        [HttpPost]
        public List<GetUserResponse> SearchUsers([FromBody] SearchUsersRequest searchUsersRequest)
        {
            if (searchUsersRequest != null)
            {
                return _UserLogic.SearchUsers(searchUsersRequest.SearchValue).Select(s => new GetUserResponse(s)).ToList();
            }
            return null;
        }

        [HttpPost]
        public GetUserResponse AddUser([FromBody] AddUserRequest addUserRequest)
        {
            if (addUserRequest != null)
            {
                var addedUser = _UserLogic.AddUser(addUserRequest.FirstName, addUserRequest.LastName, addUserRequest.GroupIds);
                return new GetUserResponse(addedUser);
            }
            return null;
        }

        [HttpPost]
        public bool EditUser([FromBody] EditUserRequest editUserRequest)
        {
            if (editUserRequest != null)
            {
                var addedUser = _UserLogic.EditUser(editUserRequest.UserId, editUserRequest.FirstName, editUserRequest.LastName);
                return (addedUser != null);
            }
            return false;
        }

        [HttpPost]
        public bool AssingGroupsToUser([FromBody] AssignGroupsToUserRequest assignGroupsToUser)
        {
            if (assignGroupsToUser != null)
            {
                var addedUser = _UserLogic.AssignGroupsToUser(assignGroupsToUser.UserId, assignGroupsToUser.GroupIds);
                return (addedUser != null);
            }
            return false;
        }

        [HttpPost]
        public bool RemoveUser([FromBody] DefaultRequest removeUserRequest)
        {
            if (removeUserRequest != null)
            {
                var addedUser = _UserLogic.RemoveUser(removeUserRequest.UserId);
                return (addedUser != null);
            }
            return false;
        }
    }
}
