using System;
using System.Collections.Generic;

namespace ElvenCurse.Core.Model.Npcs
{
    public class Bunny:Npc
    {
        private readonly Random _rnd;
        //private Random _rnd = new Random();

        public Bunny(Random rnd) : base(3, 3, Npctype.Bunny)
        {
            _rnd = rnd;
        }

        public override void CalculateNextMove(List<Character> characters)
        {
            MoveRandomly(_rnd);
        }

        public override void Attack(Character characterToAttack)
        {
            throw new NotImplementedException();
        }
    }
}
