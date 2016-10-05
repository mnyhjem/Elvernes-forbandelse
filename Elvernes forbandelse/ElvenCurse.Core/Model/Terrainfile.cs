using ElvenCurse.Core.Model.Tilemap;

namespace ElvenCurse.Core.Model
{
    public class Terrainfile
    {
        public int Id { get; set; }
        public string Filename { get; set; }
        public Tileset Tileset { get; set; }
    }
}
