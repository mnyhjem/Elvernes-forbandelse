using System;

namespace ElvenCurse.Core.Engines.Messagequeue
{
    public class Queueelement
    {
        public int Id { get; set; }
        public Messagetype Type { get; set; }
        public string Parameters { get; set; }
        public DateTime Queuetime { get; set; }
    }
}
