using RunGroopWebApp.Models;

namespace RunGroopWebApp.ViewModels
{
    public class DashboardViewModel
    {
        public List<Race> Races { get; set; } = new List<Race>();
        public List<Club> Clubs { get; set; } = new List<Club>();
    }
}
