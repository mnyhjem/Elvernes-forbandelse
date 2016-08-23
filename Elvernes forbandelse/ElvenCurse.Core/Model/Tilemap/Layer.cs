using System;
using System.Collections.Generic;

namespace ElvenCurse.Core.Model.Tilemap
{
    [Serializable]
    public class Layer
    {
        public List<int> data { get; set; }
        public int height { get; set; }
        public string name { get; set; }
        public int opacity { get; set; }
        public string type { get; set; }
        public bool visible { get; set; }
        public int width { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public Properties properties { get; set; }
        public Propertytypes propertytypes { get; set; }
    }
}
