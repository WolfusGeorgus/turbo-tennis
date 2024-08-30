namespace TurboTennisApp.Components.Backend.Models
{
    public class Match
    {
        public int Id { get; set; }
        public MatchType Type { get; set; }
        public int MatchTypeId {  get; set; }
        public bool Finished { get; set; }
        public DateTime DateTime { get; set; }
        public Tournament Tournament { get; set; }  
        public int TournamentId {  get; set; }
        public List<Set> Sets;
    }
}
