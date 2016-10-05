using System;
using System.Xml.Serialization;

namespace ElvenCurse.Core.Model.Tilemap
{
    [Serializable]
    public class Terrain
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("tile")]
        public string Tile { get; set; }
    }
}
