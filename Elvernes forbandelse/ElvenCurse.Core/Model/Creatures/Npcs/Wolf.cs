using System;
using System.Collections.Generic;

namespace ElvenCurse.Core.Model.Creatures.Npcs
{
    public class Wolf : NpcBase
    {
        public Wolf(Random rnd) : base(5, 2, Npctype.Wolf)
        {
            Rnd = rnd;
            Mode = Creaturemode.Hostile;

            Movetype = CreatureMovetype.Random;
        }

        public override bool Attack(Character characterToAttack)
        {
            if (characterToAttack != null)
            {
                LastCharacterAttacked = characterToAttack;
            }

            // hvis vi er længere væk ens angrebsafstanden, skal vi gå tættere på
            if (!IsWithinDistance(CurrentLocation, LastCharacterAttacked.Location, AttackDistance))
            {
                MoveTowardsLocation(LastCharacterAttacked.Location);
                return true;
            }

            // angrib.


            return false;
        }
    }
}
