using System;
using System.Xml.Serialization;

namespace ElvenCurse.Core.Model.Tilemap
{
    [Serializable]
    public class Terraintypes
    {
        [XmlElement("terrain")]
        public Terrain[] Terrains { get; set; }
    }
}
