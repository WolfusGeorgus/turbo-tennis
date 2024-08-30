namespace TurboTennisApp.Components.Backend.Models
{
    public class Group
    {
        public int Id { get; set; }
        public List<Player> Players { get; set; }
        public Tournament Tournament { get; set; }
        public int TournamentId {  get; set; }
    }
}
