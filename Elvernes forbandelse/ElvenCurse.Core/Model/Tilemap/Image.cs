using System;
using System.Xml.Serialization;

namespace ElvenCurse.Core.Model.Tilemap
{
    [Serializable]
    public class Image
    {
        [XmlAttribute("source")]
        public string Source { get; set; }
        [XmlAttribute("width")]
        public int Width { get; set; }
        [XmlAttribute("height")]
        public int Height { get; set; }
        [XmlAttribute("trans")]
        public string Transparentcolor { get; set; }
    }
}
