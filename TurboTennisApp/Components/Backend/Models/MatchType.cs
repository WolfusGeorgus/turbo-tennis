namespace TurboTennisApp.Components.Backend.Models
{
    public class MatchType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Match> Matches { get; set; }
    }
}
