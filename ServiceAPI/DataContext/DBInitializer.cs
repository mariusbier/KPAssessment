using ServiceAPI.Models;

namespace ServiceAPI.DataContext
{
    public class DBInitializer
    {
        public void InitializeDatabase(DatabaseContext databaseContext)
        {
            databaseContext.Users.Add(new User()
            {
                FirstName = "Marius",
                LastName = "Bierman"
            });

            List<Group> groups = new List<Group>
            {
                new Group()
                {
                    Name = "Administrator"
                },
                new Group()
                {
                    Name = "Developer"
                },
                new Group()
                {
                    Name = "Tester"
                }
            };
            groups.ForEach(group => databaseContext.Groups.Add(group));

            List<Permission> permissions = new List<Permission>
            {
                new Permission()
                {
                    Name = "Add"
                },
                new Permission()
                {
                    Name = "Edit"
                },
                new Permission()
                {
                    Name = "Update"
                },
                new Permission()
                {
                    Name = "Delete"
                }
            };
            permissions.ForEach(permission => databaseContext.Permissions.Add(permission));

            databaseContext.SaveChanges();
        }
    }
}
