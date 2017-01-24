using System.Linq;
using ElvenCurse.Core.Model.Creatures;
using ElvenCurse.Core.Model.Creatures.Npcs;
using ElvenCurse.Core.Utilities;

namespace ElvenCurse.Core.Model
{
    public class Character : Creature
    {
        public Character(Creaturetype type) : base(type, 0, 0)
        {
            IsPlayer = true;
        }

        public string LastAttackerror { get; private set; }
        
        public string ConnectionId { get; set; }

        public Connectionstatus Connectionstatus { get; set; }

        public int AccumulatedExperience { get; set; }

        public override int Level
        {
            get { return ExperienceCalculations.CurrentlevelFromAccumulatedXp(AccumulatedExperience); }
        }
        
        public override bool Attack(Creature characterToAttack, int activatedAbility)
        {
            if (characterToAttack == null)
            {
                characterToAttack = this;
            }

            if (!IsAlive)
            {
                LastAttackerror = "Du er død...";
                return false;
            }

            if (!characterToAttack.IsAlive)
            {
                LastAttackerror = "Dit mål er død";
                return false;
            }

            var abilityToUse = Abilities[activatedAbility];
            if (abilityToUse == null)
            {
                LastAttackerror = "Du kender ikke dette angreb";
                return false;
            }

            // hvis vi er længere væk ens angrebsafstanden, skal vi gå tættere på
            if (!Location.IsWithinReachOf(characterToAttack.Location, abilityToUse.AttackDistance))
            {
                LastAttackerror = "Du er for langt væk";
                return false;
            }
            
            abilityToUse.Use(characterToAttack);

            return true;
        }
    }

    public enum Connectionstatus
    {
        Offline = 0,
        Online = 1,
        Away = 2
    }
}
