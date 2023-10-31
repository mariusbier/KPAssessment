using Client.Models;

namespace Client.ViewModels
{
    public class Summary
    {
        public User User { get; set; }
        public List<Group> Groups { get; set; }
        public List<Guid> SelectedGroups { get; set; }
    }
}
