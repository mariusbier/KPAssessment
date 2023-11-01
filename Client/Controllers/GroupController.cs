using Client.Models;
using Client.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace Client.Controllers
{
    public class GroupController : Controller
    {
        private readonly ILogger<GroupController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private static Group _selectedGroup;
        private static Permission _selectedPermission;
        private static List<Permission> _permissions;
        private static List<Guid> _selectedPermissions;

        public GroupController(ILogger<GroupController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> ListGroups()
        {
            var groups = new List<Group>();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7213/Group/GetAllGroups");
                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    groups = await response.Content.ReadFromJsonAsync<List<Group>>();
                }
                else
                {
                    groups = new List<Group>(); // or any appropriate error handling
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to load all groups", e);
            }
            return View(groups);
        }

        public async Task<IActionResult> EditGroup(Guid groupId)
        {
            try
            {
                await LoadPermissions();

                if (groupId != Guid.Empty)
                {
                    var groupRequest = new GetGroupRequest()
                    {
                        GroupId = groupId,
                        IncludeUsers = true,
                        IncludePermissions = true,
                    };

                    var client = _clientFactory.CreateClient();

                    var response = await client.PostAsJsonAsync<GetGroupRequest>("https://localhost:7213/Group/GetGroup", groupRequest);

                    if (response.IsSuccessStatusCode)
                    {
                        var group = await response.Content.ReadFromJsonAsync<Group>();
                        _selectedGroup = group;
                    }
                }
                else
                {
                    _selectedGroup = new Group();
                }
                if (_selectedPermissions == null)
                {
                    _selectedPermissions = _selectedGroup.Permissions?.Select(s => s.PermissionId).Intersect(_permissions.Select(s => s.PermissionId)).ToList() ?? new List<Guid>();
                }
                var viewModel = new EditGroup()
                {
                    Group = _selectedGroup,
                    Permissions = _permissions,
                    SelectedPermissions = _selectedPermissions ?? new List<Guid>()
                };
                return View(viewModel);
            }
            catch(Exception e)
            {
                _logger.LogError($"Failed to load Group: {groupId}", e);
            }
            return View(new Group());
        }

        public async Task<IActionResult> SaveGroup(Group group)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                if (group.GroupId == Guid.Empty)
                {
                    var response = await client.PostAsJsonAsync<Group>("https://localhost:7213/Group/AddGroup", group);

                    if (response.IsSuccessStatusCode)
                    {
                        group = await response.Content.ReadFromJsonAsync<Group>();
                    }
                }
                else
                {
                    var response = await client.PostAsJsonAsync<Group>("https://localhost:7213/Group/EditGroup", group);

                    if (response.IsSuccessStatusCode)
                    {
                        await response.Content.ReadFromJsonAsync<bool>();
                    }
                }

                var assignPermissionRequest = new AssignPermissionsToGroupRequest
                {
                    GroupId = group.GroupId,
                    PermissionIds = _selectedPermissions
                };
                var groupsResponse = await client.PostAsJsonAsync<AssignPermissionsToGroupRequest>("https://localhost:7213/Group/AssignPermissionsToGroup", assignPermissionRequest);

                if (groupsResponse.IsSuccessStatusCode)
                {
                    await groupsResponse.Content.ReadFromJsonAsync<bool>();
                }
                _selectedGroup = null;
                _selectedPermissions = null;
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to save group", e);
            }
            return Redirect("/Group/ListGroups");
        }

        public async Task<IActionResult> RemoveGroup(Guid groupId)
        {
            try
            {
                var request = new DefaultRequest()
                {
                    GroupId = groupId
                };
                var client = _clientFactory.CreateClient();

                var response = await client.PostAsJsonAsync<DefaultRequest>("https://localhost:7213/Group/RemoveGroup", request);

                if (response.IsSuccessStatusCode)
                {
                    await response.Content.ReadFromJsonAsync<bool>();
                }

                _selectedGroup = null;
                _selectedPermissions = null;
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to remove group", e);
            }
            return Redirect("/Group/ListGroups");
        }

        public IActionResult AssignPermission(Guid permissionId)
        {
            _selectedPermissions.Add(permissionId);
            return Redirect($"/Group/EditGroup?groupId={_selectedGroup.GroupId}");
        }
        public IActionResult UnassignPermission(Guid permissionId)
        {
            _selectedPermissions.Remove(permissionId);
            return Redirect($"/Group/EditGroup?groupId={_selectedGroup.GroupId}");
        }

        public async Task<IActionResult> ListPermissions()
        {
            var permissions = new List<Permission>();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7213/Group/GetAllPermissions");
                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    permissions = await response.Content.ReadFromJsonAsync<List<Permission>>();
                }
                else
                {
                    permissions = new List<Permission>(); // or any appropriate error handling
                }


            }
            catch (Exception e)
            {
                _logger.LogError("Failed to load all permissions", e);
            }
            return View(permissions);
        }

        public async Task<IActionResult> EditPermission(Guid permissionId)
        {
            try
            {
                if (permissionId != Guid.Empty)
                {
                    var permissionRequest = new GetPermissionRequest()
                    {
                        PermissionId = permissionId,
                        IncludeGroups = true
                    };

                    var client = _clientFactory.CreateClient();

                    var response = await client.PostAsJsonAsync<GetPermissionRequest>("https://localhost:7213/Group/GetPermission", permissionRequest);

                    if (response.IsSuccessStatusCode)
                    {
                        var permission = await response.Content.ReadFromJsonAsync<Permission>();
                        _selectedPermission = permission;
                    }
                }
                else
                {
                    _selectedPermission = new Permission();
                }
                var viewModel = new EditPermission()
                {
                    Permission = _selectedPermission
                };
                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to load Permission: {permissionId}", e);
            }
            return View(new EditPermission());
        }

        public async Task<IActionResult> SavePermission(Permission permission)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                if (permission.PermissionId == Guid.Empty)
                {
                    var response = await client.PostAsJsonAsync<Permission>("https://localhost:7213/Group/AddPermission", permission);

                    if (response.IsSuccessStatusCode)
                    {
                        permission = await response.Content.ReadFromJsonAsync<Permission>();
                    }
                }
                else
                {
                    var response = await client.PostAsJsonAsync<Permission>("https://localhost:7213/Group/EditPermission", permission);

                    if (response.IsSuccessStatusCode)
                    {
                        await response.Content.ReadFromJsonAsync<bool>();
                    }
                }

                _selectedGroup = null;
                _selectedPermission = null;
                _selectedPermissions = null;
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to save Permission", e);
            }
            return Redirect("/Group/ListPermissions");
        }

        public async Task<IActionResult> RemovePermission(Guid permissionId)
        {
            try
            {
                var request = new DefaultRequest()
                {
                    PermissionId = permissionId
                };
                var client = _clientFactory.CreateClient();

                var response = await client.PostAsJsonAsync<DefaultRequest>("https://localhost:7213/Group/RemovePermission", request);

                if (response.IsSuccessStatusCode)
                {
                    await response.Content.ReadFromJsonAsync<bool>();
                }

                _selectedGroup = null;
                _selectedPermission = null;
                _selectedPermissions = null;
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to remove permission", e);
            }
            return Redirect("/Group/ListPermissions");
        }

        private async Task<List<Permission>> LoadPermissions()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7213/Group/GetAllPermissions");
            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            _permissions = new List<Permission>();

            if (response.IsSuccessStatusCode)
            {
                _permissions = await response.Content.ReadFromJsonAsync<List<Permission>>();
            }
            return _permissions;
        }
    }
}
