using Client.Models;

namespace Client.ViewModels
{
    public class EditGroup
    {
        public Group Group { get; set; }
        public List<Permission> Permissions { get; set; }
        public List<Guid> SelectedPermissions { get; set; }
    }
}
