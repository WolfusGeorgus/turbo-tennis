namespace TurboTennisApp.Components.Backend.Models
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public List<Player> Players { get; set; }
        public List<Group> Groups { get; set; }
        public List<Match> Matches { get; set; }
    }
}
