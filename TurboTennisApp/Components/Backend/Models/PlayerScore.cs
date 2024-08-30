namespace TurboTennisApp.Components.Backend.Models
{
    public class PlayerScore
    {
        public int Id { get; set; }
        public Player Player {  get; set; }
        public int PlayerId {  get; set; }
        public int Score { get; set; }
        public Set Set {  get; set; }
        public int SetId {  get; set; }
    }
}
