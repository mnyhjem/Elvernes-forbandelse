namespace ElvenCurse.Core.Model.Creatures.Npcs
{
    public class HunterNpc : NpcBase
    {
        public HunterNpc():base(5, 5, Creaturetype.Hunter)
        {
            Mode = Creaturemode.FactionDepending;
            Movetype = CreatureMovetype.FollowCharactor;
        }

        public override bool Attack(Creature characterToAttack, int activatedAbility)
        {
            if (characterToAttack != null)
            {
                LastCreatureAttacked = characterToAttack;
            }

            // hvis vi er længere væk ens angrebsafstanden, skal vi gå tættere på

            // angrib.

            return false;
        }
    }
}