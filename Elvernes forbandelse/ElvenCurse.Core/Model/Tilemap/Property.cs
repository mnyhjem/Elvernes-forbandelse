using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace ElvenCurse.Core.Model.Tilemap
{
    [Serializable]
    public class Property
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }

        [DefaultValue("string")]
        [XmlAttribute("type")]
        public string Type { get; set; }

        public Property()
        {
            if (string.IsNullOrEmpty(Type))
            {
                Type = "string";
            }
        }
    }
}
