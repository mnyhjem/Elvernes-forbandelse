namespace ElvenCurse.Core.Model
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }

        public string ConnectionId { get; set; }
    }
}
