using System;
using System.Diagnostics;
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
                Trace.WriteLine($"{Name} løber efter {LastCreatureAttacked.Name}");
                MoveTowardsLocation(LastCreatureAttacked.Location);
                return true;
            }

            // angrib.
            if (!Abilities.Any())
            {
                Trace.WriteLine($"{Name} har ingen våben");
                return false;
            }

            if (!LastCreatureAttacked.IsAlive)
            {
                //Trace.WriteLine(string.Format("{0}", LastCharacterAttacked.Health));
                Action = CreatureAction.ReturnToDefaultLocation;
                return false;
            }

            // find et angrib vi vil bruge..
            Trace.WriteLine($"{Name} angriber {LastCreatureAttacked.Name}");

            var damageAbilities = Abilities.Where(a => !a.Passive && !a.IsHeal).ToList();
            var ability = damageAbilities[Rnd.Next(damageAbilities.Count - 1)];
            string msg;
            ability.Use(characterToAttack, out msg);

            return true;
        }
    }
}
