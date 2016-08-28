using System.Collections.Generic;

namespace ElvenCurse.Core.Model.InteractiveObjects
{
    public abstract class InteractiveObject
    {
        public int Id { get; set; }
        public InteractiveobjectType Type { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }

        public abstract InteractiveobjectResult Interact(Character character);

        public List<InteractiveobjectParameter> Parameters { get; set; }
    }
}
