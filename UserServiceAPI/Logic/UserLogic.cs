using Microsoft.EntityFrameworkCore;
using UserServiceAPI.DataContext;
using UserServiceAPI.DataContext.Models;
using UserServiceAPI.ViewModels;

namespace UserServiceAPI.Logic
{
    public class UserLogic
    {
        private DatabaseContext DatabaseContext;
        public UserLogic()
        {
        }

        public UserLogic(DatabaseContext databaseContext) {
            DatabaseContext = databaseContext;
        }

        public User GetUser(Guid userId, bool includeGroups = false) {
            try
            {
                var query = DatabaseContext.Users.AsQueryable();
                if (includeGroups)
                {
                    query = query.Include(i => i.Groups).AsQueryable();
                }
                var user = query.SingleOrDefault(x => x.UserId == userId);

                return user;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }

        public List<User> GetAllUsers() {
            try
            {
                var users = DatabaseContext.Users.ToList();
                
                return users;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }

        public int CountAllUsers() {
            try
            {
                var NumberOfUsers = DatabaseContext.Users.Count();
                
                return NumberOfUsers;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return 0;
        }

        public User AddUser(string firstName, string lastName, List<Guid> groupIds) {
            try
            {
                var groups = new List<Group>();
                if (groupIds != null && groupIds.Count > 0)
                {
                    groups = new GroupLogic(DatabaseContext).GetGroups(groupIds);
                }
                var newUser = new User() 
                { 
                    UserId = Guid.NewGuid(), 
                    FirstName = firstName, 
                    LastName = lastName,
                    Groups = groups
                };
                DatabaseContext.Users.Add(newUser);
                DatabaseContext.SaveChanges();
                return newUser;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }

        public User EditUser(Guid userId, string firstName, string lastName)
        {
            try
            {
                var user = GetUser(userId);
                if (user != null)
                {
                    user.FirstName = firstName;
                    user.LastName = lastName;

                    DatabaseContext.Users.Update(user);
                    DatabaseContext.SaveChanges();
                    return user;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }

        public bool RemoveUser(Guid userId) {
            try
            {
                var user = GetUser(userId, false);
                if (user != null)
                {
                    DatabaseContext.Users.Remove(user);
                    DatabaseContext.SaveChanges();
                    return true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return false;
        }

        public List<User> SearchUsers(string searchValue)
        {
            try
            {
                var users = DatabaseContext.Users.Where(x => x.FirstName == searchValue || x.LastName == searchValue).ToList();
                return users;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }

        public List<Group> GetUserGroups(Guid userId)
        {
            try
            {
                var userGroups = DatabaseContext.Users.Select(s => new { s.UserId, s.Groups }).Where(x => x.UserId == userId).FirstOrDefault();

                return userGroups.Groups;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }

        public User AssignGroupsToUser(Guid userId, List<Guid> groupIds)
        {
            try
            {
                var groups = new List<Group>();
                if (groupIds.Count > 0)
                {
                    groups = new GroupLogic(DatabaseContext).GetGroups(groupIds);
                }
                var user = GetUser(userId, true);
                user.Groups = groups;
                //DatabaseContext.Users.Update(user);
                DatabaseContext.SaveChanges();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return null;
        }
    }
}
