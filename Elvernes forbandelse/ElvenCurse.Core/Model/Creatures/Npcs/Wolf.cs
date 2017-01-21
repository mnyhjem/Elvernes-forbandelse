using System;
using System.Linq;
using ElvenCurse.Core.Utilities;

namespace ElvenCurse.Core.Model.Creatures.Npcs
{
    public class Wolf : NpcBase
    {
        public Wolf(Random rnd) : base(5, 2, Creaturetype.Wolf)
        {
            Rnd = rnd;
            Mode = Creaturemode.Hostile;

            Movetype = CreatureMovetype.Random;

            Abilities.Add(new CreatureAbility(this)
            {
                Name = "Bid",
                Cooldown = 3,
                BaseDamage = 30
            });
        }

        public override bool Attack(Creature characterToAttack, int activatedAbility)
        {
            if (characterToAttack != null)
            {
                LastCreatureAttacked = characterToAttack;
            }

            // hvis vi er længere væk ens angrebsafstanden, skal vi gå tættere på
            if (!Location.IsWithinReachOf(LastCreatureAttacked.Location, AttackDistance))
            {
                MoveTowardsLocation(LastCreatureAttacked.Location);
                return true;
            }

            // angrib.
            if (!Abilities.Any())
            {
                return false;
            }

            if (!LastCreatureAttacked.IsAlive)
            {
                //Trace.WriteLine(string.Format("{0}", LastCharacterAttacked.Health));
                Action = CreatureAction.ReturnToDefaultLocation;
                return false;
            }

            // find et angrib vi vil bruge..
            var damageAbilities = Abilities.Where(a => !a.Passive && !a.IsHeal).ToList();
            var ability = damageAbilities[Rnd.Next(damageAbilities.Count - 1)];
            ability.Use(characterToAttack);

            return true;
        }
    }
}
