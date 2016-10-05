using System;
using System.Xml.Serialization;

namespace ElvenCurse.Core.Model.Tilemap
{
    [Serializable]
    public class PropertyCollection
    {
        [XmlElement("property")]
        public Property[] Properties { get; set; }
    }
}
