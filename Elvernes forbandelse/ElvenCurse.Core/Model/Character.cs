namespace ElvenCurse.Core.Model
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }

        public string ConnectionId { get; set; }

        public Connectionstatus Connectionstatus { get; set; }

        public int AccumulatedExperience { get; set; }

        public int Level
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
