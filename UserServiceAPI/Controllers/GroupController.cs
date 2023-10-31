using Microsoft.AspNetCore.Mvc;
using UserServiceAPI.DataContext;
using UserServiceAPI.Helpers;
using UserServiceAPI.Logic;
using UserServiceAPI.ViewModels;

namespace UserServiceAPI.Controllers
{
    [ApiController]
    [Route("/[controller]/[action]")]
    [Produces("application/json"), Consumes("application/json")]
    public class GroupController : ControllerBase
    {
        private GroupLogic _GroupLogic;
        private static CachedDictionary<string, object> Cache = new CachedDictionary<string, object>(new TimeSpan(0, 15, 0));

        public GroupController(
            DatabaseContext databaseContext)
            : base()
        {
            _GroupLogic = new GroupLogic(databaseContext);
        }

        [HttpPost]
        public GetGroupResponse GetGroup([FromBody] GetGroupRequest groupRequest)
        {
            if (groupRequest != null && groupRequest.GroupId != new Guid())
            {
                var group = new GetGroupResponse(_GroupLogic.GetGroup(groupRequest.GroupId, groupRequest.IncludeUsers, groupRequest.IncludePermissions));
                return group;
            }
            return null;
        }

        [HttpGet]
        public List<GetGroupResponse> GetAllGroups()
        {
            return _GroupLogic.GetAllGroups().Select(s => new GetGroupResponse(s)).ToList();
        }

        [HttpGet]
        public int GetGroupCount()
        {
           return _GroupLogic.CountAllGroups();
        }

        [HttpGet]
        public int GetUserCountForGroup(Guid groupId)
        {
           return _GroupLogic.CountUsersForGroup(groupId);
        }

        [HttpGet]
        public int GetPermissionCountForGroup(Guid groupId)
        {
           return _GroupLogic.CountPermissionsForGroup(groupId);
        }

        [HttpPost]
        public List<GetPermissionResponse> GetGroupPermissions([FromBody] DefaultRequest groupRequest)
        {
            if (groupRequest != null && groupRequest.GroupId != new Guid())
            {
                var permissions = _GroupLogic.GetGroupPermissions(groupRequest.GroupId).Select(s => new GetPermissionResponse(s)).ToList();
                return permissions;
            }
            return null;
        }

        [HttpPost]
        public GetGroupResponse AddGroup([FromBody] AddGroupRequest addGroupRequest)
        {
            if (addGroupRequest != null)
            {
                var addedGroup = _GroupLogic.AddGroup(addGroupRequest.Name, addGroupRequest.PermissionIds);
                return new GetGroupResponse(addedGroup);
            }
            return null;
        }

        [HttpPost]
        public bool EditGroup([FromBody] EditGroupRequest editGroupRequest)
        {
            if (editGroupRequest != null)
            {
                var editedGroup = _GroupLogic.EditGroup(editGroupRequest.GroupId, editGroupRequest.Name);
                return (editedGroup != null);
            }
            return false;
        }

        [HttpPost]
        public bool AssignPermissionsToGroup([FromBody] AssignPermissionsToGroupRequest assignPermissionsToGroup)
        {
            if (assignPermissionsToGroup != null)
            {
                var assignedGroup = _GroupLogic.AssignPermissionsToGroup(assignPermissionsToGroup.GroupId, assignPermissionsToGroup.PermissionIds);
                return (assignedGroup != null);
            }
            return false;
        }

        [HttpPost]
        public bool RemoveGroup([FromBody] DefaultRequest removeGroupRequest)
        {
            if (removeGroupRequest != null)
            {
                var removedGroup = _GroupLogic.RemoveGroup(removeGroupRequest.GroupId);
                return (removedGroup != null);
            }
            return false;
        }

        [HttpPost]
        public GetPermissionResponse GetPermission([FromBody] GetPermissionRequest permissionRequest)
        {
            if (permissionRequest != null && permissionRequest.PermissionId != new Guid())
            {
                var group = new GetPermissionResponse(_GroupLogic.GetPermission(permissionRequest.PermissionId, permissionRequest.IncludeGroups));
                return group;
            }
            return null;
        }

        [HttpPost]
        public GetPermissionResponse AddPermission([FromBody] AddPermissionRequest addPermissionRequest)
        {
            if (addPermissionRequest != null)
            {
                var addedPermission = _GroupLogic.AddPermission(addPermissionRequest.Name);
                return new GetPermissionResponse(addedPermission);
            }
            return null;
        }

        [HttpPost]
        public bool EditPermission([FromBody] EditPermissionRequest editPermissionRequest)
        {
            if (editPermissionRequest != null)
            {
                var editedPermission = _GroupLogic.EditPermission(editPermissionRequest.PermissionId, editPermissionRequest.Name);
                return (editedPermission != null);
            }
            return false;
        }

        [HttpPost]
        public bool RemovePermission([FromBody] DefaultRequest removePermissionRequest)
        {
            if (removePermissionRequest != null)
            {
                var removedPermission = _GroupLogic.RemovePermission(removePermissionRequest.PermissionId);
                return (removedPermission != null);
            }
            return false;
        }

        [HttpGet]
        public List<GetPermissionResponse> GetAllPermissions()
        {
            return _GroupLogic.GetAllPermissions().Select(s => new GetPermissionResponse(s)).ToList();
        }
    }
}
