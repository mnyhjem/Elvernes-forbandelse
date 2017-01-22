using System.Runtime.Serialization;

namespace ElvenCurse.Core.Model.Creatures
{
    public class AffectedByAbility
    {
        [IgnoreDataMember]
        public Creature DoneBy { get; set; }
        public string Name { get; set; }
        public int Healtheffect { get; set; }
    }
}
