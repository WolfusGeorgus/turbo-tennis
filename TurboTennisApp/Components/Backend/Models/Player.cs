namespace TurboTennisApp.Components.Backend.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public List<Group> Groups { get; set; }
        public List<Tournament> Tournaments { get; set; }

    }
}
