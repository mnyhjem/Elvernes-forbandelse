using ElvenCurse.Core.Model.Creatures;

namespace ElvenCurse.Core.Model
{
    public class Character : Creature
    {
        public Character() : base(0, 0)
        {
        }

        public override bool Attack(Character characterToAttack)
        {
            return false;
        }

        //public int Id { get; set; }
        //public string Name { get; set; }
        public Location Location { get; set; }

        public string ConnectionId { get; set; }

        public Connectionstatus Connectionstatus { get; set; }

        public int AccumulatedExperience { get; set; }

        public override int Level
        {
            get { return Utilities.ExperienceCalculations.CurrentlevelFromAccumulatedXp(AccumulatedExperience); }
        }
    }

    public enum Connectionstatus
    {
        Offline = 0,
        Online = 1,
        Away = 2
    }
}
