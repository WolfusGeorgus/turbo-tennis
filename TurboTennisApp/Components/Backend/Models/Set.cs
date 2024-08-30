namespace TurboTennisApp.Components.Backend.Models
{
    public class Set
    {
        public int Id { get; set; }
        public int GameId {  get; set; }
        public Match Match { get; set; }
        public int MatchId { get; set; }
        public List<PlayerScore> PlayerScores { get; set; }
    }
}
