using System;
using System.Diagnostics;
using System.Linq;
using ElvenCurse.Core.Utilities;

namespace ElvenCurse.Core.Model.Creatures.Npcs
{
    public class Wolf : NpcBase
    {
        public Wolf(Random rnd) : base(5, 2, Npctype.Wolf)
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

        public override bool Attack(Character characterToAttack)
        {
            if (characterToAttack != null)
            {
                LastCharacterAttacked = characterToAttack;
            }

            // hvis vi er længere væk ens angrebsafstanden, skal vi gå tættere på
            if (!CurrentLocation.IsWithinReachOf(LastCharacterAttacked.Location, AttackDistance))
            {
                MoveTowardsLocation(LastCharacterAttacked.Location);
                return true;
            }

            // angrib.
            if (!Abilities.Any())
            {
                return false;
            }

            if (!LastCharacterAttacked.IsAlive)
            {
                //Trace.WriteLine(string.Format("{0}", LastCharacterAttacked.Health));
                Action = CreatureAction.ReturnToDefaultLocation;
                return false;
            }

            // find et angrib vi vil bruge..
            var ability = Abilities[Rnd.Next(Abilities.Count - 1)];
            ability.Use(characterToAttack);

            return true;
        }
    }
}
