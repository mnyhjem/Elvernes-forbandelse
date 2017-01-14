using System;
using System.Collections.Generic;

namespace ElvenCurse.Core.Model.Creatures.Npcs
{
    public class Bunny:NpcBase
    {
        public Bunny(Random rnd) : base(3, 3, Npctype.Bunny)
        {
            Rnd = rnd;

            Mode = Creaturemode.Friendly;

            Movetype = CreatureMovetype.Random;
        }
    }
}
