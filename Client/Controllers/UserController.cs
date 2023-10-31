using Client.Models;
using Client.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Net.Http.Json;

namespace Client.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private static User _selectedUser;
        private static List<Group> _groups;
        private static List<Guid> _selectedGroups;

        public UserController(ILogger<UserController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> ListUsers()
        {
            var users = new List<User>();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7213/User/GetAllUsers");
                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    users = await response.Content.ReadFromJsonAsync<List<User>>();
                }
                else
                {
                    // Handle unsuccessful response
                    users = new List<User>(); // or any appropriate error handling
                }

                _selectedUser = null;
                _selectedGroups = null;
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to load all users", e);
            }
            return View(users);
        }

        public async Task<IActionResult> EditUser(Guid userId)
        {
            try
            {
                await LoadGroups();

                if (userId != Guid.Empty)
                {
                    var userRequest = new GetUserRequest()
                    {
                        UserId = userId,
                        IncludeGroups = true
                    };

                    var client = _clientFactory.CreateClient();

                    var response = await client.PostAsJsonAsync<GetUserRequest>("https://localhost:7213/User/GetUser", userRequest);

                    if (response.IsSuccessStatusCode)
                    {
                        var user = await response.Content.ReadFromJsonAsync<User>();
                        _selectedUser = user;
                    }
                }
                else
                {
                    _selectedUser = new User();
                }
                if (_selectedGroups == null)
                {
                    _selectedGroups = _selectedUser.Groups?.Select(s => s.GroupId).Intersect(_groups.Select(s => s.GroupId)).ToList() ?? new List<Guid>();
                }
                var viewModel = new EditUser() {
                    User = _selectedUser,
                    Groups = _groups,
                    SelectedGroups = _selectedGroups ?? new List<Guid>()
                };
                return View(viewModel);
            }
            catch(Exception e)
            {
                _logger.LogError($"Failed to load user: {userId}", e);
            }
            return View(new User());
        }

        public async Task<IActionResult> SaveUser(User user)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                
                if (user.UserId == Guid.Empty)
                {
                    var response = await client.PostAsJsonAsync<User>("https://localhost:7213/User/AddUser", user);

                    if (response.IsSuccessStatusCode)
                    {
                        user = await response.Content.ReadFromJsonAsync<User>();
                    }
                } 
                else
                {
                    var response = await client.PostAsJsonAsync<User>("https://localhost:7213/User/EditUser", user);

                    if (response.IsSuccessStatusCode)
                    {
                        await response.Content.ReadFromJsonAsync<bool>();
                    }
                }

                var assignGroupRequest = new AssignGroupsToUserRequest
                {
                    UserId = user.UserId,
                    GroupIds = _selectedGroups
                };
                var groupsResponse = await client.PostAsJsonAsync<AssignGroupsToUserRequest>("https://localhost:7213/User/AssingGroupsToUser", assignGroupRequest);

                if (groupsResponse.IsSuccessStatusCode)
                {
                    await groupsResponse.Content.ReadFromJsonAsync<bool>();
                }
                _selectedUser = null;
                _selectedGroups = null;
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to save user", e);
            }
            return Redirect("/");
        }

        public async Task<IActionResult> RemoveUser(Guid userId)
        {
            try
            {
                var request = new DefaultRequest()
                {
                    UserId = userId
                };
                var client = _clientFactory.CreateClient();

                var response = await client.PostAsJsonAsync<DefaultRequest>("https://localhost:7213/User/RemoveUser", request);

                if (response.IsSuccessStatusCode)
                {
                    await response.Content.ReadFromJsonAsync<bool>();
                }
                
                _selectedUser = null;
                _selectedGroups = null;
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to save user", e);
            }
            return Redirect("/");
        }

        public IActionResult AssignGroup(Guid groupId)
        {
            _selectedGroups.Add(groupId);
            return Redirect($"/User/EditUser?userId={_selectedUser.UserId}");
        }
        public IActionResult UnassignGroup(Guid groupId)
        {
            _selectedGroups.Remove(groupId);
            return Redirect($"/User/EditUser?userId={_selectedUser.UserId}");
        }

        private async Task<List<Group>> LoadGroups()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7213/Group/GetAllGroups");
            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            _groups = new List<Group>();

            if (response.IsSuccessStatusCode)
            {
                _groups = await response.Content.ReadFromJsonAsync<List<Group>>();
            }
            return _groups;
        }
    }
}
