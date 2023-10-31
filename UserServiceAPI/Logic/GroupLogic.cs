using Microsoft.EntityFrameworkCore;
using UserServiceAPI.DataContext;
using UserServiceAPI.DataContext.Models;

namespace UserServiceAPI.Logic
{
    public class GroupLogic
    {
        private DatabaseContext DatabaseContext;
        public GroupLogic()
        {
        }

        public GroupLogic(DatabaseContext databaseContext) {
            DatabaseContext = databaseContext;
        }

        public List<Group> GetAllGroups() {
            try
            {
                var groups = DatabaseContext.Groups.ToList();
                
                return groups;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }

        public int CountAllGroups()
        {
            try
            {
                var numberOfGroups = DatabaseContext.Groups.Count();

                return numberOfGroups;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return 0;
        }

        public int CountUsersForGroup(Guid groupId)
        {
            try
            {
                var numberOfUsersForGroups = DatabaseContext.Groups.Where(x => x.GroupId == groupId)
                    .Include(u => u.Users).Count();

                return numberOfUsersForGroups;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return 0;
        }

        public int CountPermissionsForGroup(Guid groupId)
        {
            try
            {
                var numberOfUsersForGroups = DatabaseContext.Groups.Where(x => x.GroupId == groupId)
                    .Include(u => u.Permissions).Count();

                return numberOfUsersForGroups;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return 0;
        }

        public List<Permission> GetAllPermissions() {
            try
            {
                var permissions = DatabaseContext.Permissions.ToList();
                
                return permissions;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }

        public Group AddGroup(string name, List<Guid> permissionIds) {
            try
            {
                var permissions = new List<Permission>();
                if (permissionIds != null && permissionIds.Count > 0)
                {
                    permissions = GetPermissions(permissionIds);
                }
                var newGroup = new Group() 
                { 
                    GroupId = Guid.NewGuid(), 
                    Name = name,
                    Permissions = permissions
                };
                DatabaseContext.Groups.Add(newGroup);
                DatabaseContext.SaveChanges();
                return newGroup;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }

        public Group EditGroup(Guid groupId, string name)
        {
            try
            {
                var group = GetGroup(groupId);
                if (group != null)
                {
                    group.Name = name;

                    DatabaseContext.Groups.Update(group);
                    DatabaseContext.SaveChanges();
                    return group;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }

        public bool RemoveGroup(Guid groupId)
        {
            try
            {
                var group = GetGroup(groupId);
                if (group != null)
                {
                    DatabaseContext.Groups.Remove(group);
                    DatabaseContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return false;
        }

        public Group AssignPermissionsToUser(Guid groupId, List<Guid> permissionIds)
        {
            try
            {
                var permissions = new List<Permission>();
                if (permissionIds.Count > 0)
                {
                    permissions = GetPermissions(permissionIds);
                }
                var group = GetGroup(groupId, true);
                group.Permissions = permissions;
                //DatabaseContext.Users.Update(user);
                DatabaseContext.SaveChanges();
                return group;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }

        public Group GetGroup(Guid groupId, bool includeUsers = false, bool includePermissions = false)
        {
            try
            {
                var query = DatabaseContext.Groups.AsQueryable();
                if (includeUsers)
                {
                    query = query.Include(i => i.Users).AsQueryable();
                }
                if (includePermissions)
                {
                    query = query.Include(i => i.Permissions).AsQueryable();
                }
                var group = query.SingleOrDefault(x => x.GroupId == groupId);

                return group;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }

        public List<Permission> GetGroupPermissions(Guid groupId)
        {
            try
            {
                var groupPermissions = DatabaseContext.Groups.Select(s => new { s.GroupId, s.Permissions }).Where(x => x.GroupId == groupId).FirstOrDefault();

                return groupPermissions.Permissions;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }

        public Group AssignPermissionsToGroup(Guid groupId, List<Guid> permissionIds)
        {
            try
            {
                var permissions = new List<Permission>();
                if (permissionIds.Count > 0)
                {
                    permissions = GetPermissions(permissionIds);
                }
                var group = GetGroup(groupId, true);
                group.Permissions = permissions;
                DatabaseContext.SaveChanges();
                return group;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }

        public List<Group> GetGroups(List<Guid> groupIds)
        {
            try
            {
                var groups = DatabaseContext.Groups.Where(x => groupIds.Contains(x.GroupId)).ToList();
                return groups;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }

        public Permission GetPermission(Guid permissionId, bool includeGroups = false)
        {
            try
            {
                var query = DatabaseContext.Permissions.AsQueryable();
                if (includeGroups)
                {
                    query = query.Include(i => i.Groups).AsQueryable();
                }
                var permission = query.SingleOrDefault(x => x.PermissionId == permissionId);

                return permission;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }

        public Permission AddPermission(string name)
        {
            try
            {
                var newPermission = new Permission()
                {
                    PermissionId = Guid.NewGuid(),
                    Name = name
                };
                DatabaseContext.Permissions.Add(newPermission);
                DatabaseContext.SaveChanges();
                return newPermission;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }

        public Permission EditPermission(Guid permissionId, string name)
        {
            try
            {
                var permission = GetPermission(permissionId);
                if (permission != null)
                {
                    permission.Name = name;

                    DatabaseContext.Permissions.Update(permission);
                    DatabaseContext.SaveChanges();
                    return permission;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }

        public bool RemovePermission(Guid permissionId)
        {
            try
            {
                var permission = GetPermission(permissionId);
                if (permission != null)
                {
                    DatabaseContext.Permissions.Remove(permission);
                    DatabaseContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return false;
        }

        public List<Permission> GetPermissions(List<Guid> permissionIds)
        {
            try
            {
                var permissions = DatabaseContext.Permissions.Where(x => permissionIds.Contains(x.PermissionId)).ToList();
                return permissions;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }
    }
}
