namespace ElvenCurse.Core.Model
{
    public class Worldsection
    {
        public int Id { get; set; }
        public int MapchangeRight { get; set; }
        public int MapchangeLeft { get; set; }
        public int MapchangeUp { get; set; }
        public int MapchangeDown { get; set; }
        public string Json { get; set; }
        public Tilemap.Tilemap Tilemap { get; set; }
        public string Name { get; set; }
    }
}
