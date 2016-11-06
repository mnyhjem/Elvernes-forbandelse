using System;
using System.Collections.Generic;

namespace ElvenCurse.Core.Model.Npcs
{
    public class Wolf:Npc
    {
        private Random _rnd = new Random();

        public Wolf() : base(3, 3, Npctype.Wolf)
        {
        }

        public override void CalculateNextMove(List<Character> characters)
        {
            var directions = new List<Location>();
            directions.Add(DefaultLocation);

            if (CurrentLocation.X > DefaultLocation.X - 20)
            {
                directions.Add(new Location
                {
                    WorldsectionId = DefaultLocation.WorldsectionId,
                    X = DefaultLocation.X - 20,
                    Y = DefaultLocation.Y
                });
            }
            if (CurrentLocation.X < DefaultLocation.X + 20)
            {
                directions.Add(new Location
                {
                    WorldsectionId = DefaultLocation.WorldsectionId,
                    X = DefaultLocation.X + 20,
                    Y = DefaultLocation.Y
                });
            }
            if (CurrentLocation.Y > DefaultLocation.Y - 20)
            {
                directions.Add(new Location
                {
                    WorldsectionId = DefaultLocation.WorldsectionId,
                    X = DefaultLocation.X,
                    Y = DefaultLocation.Y - 20
                });
            }
            if (CurrentLocation.Y < DefaultLocation.Y + 20)
            {
                directions.Add(new Location
                {
                    WorldsectionId = DefaultLocation.WorldsectionId,
                    X = DefaultLocation.X,
                    Y = DefaultLocation.Y + 20
                });
            }

            var direction = directions[_rnd.Next(directions.Count)];
            MoveTowardsLocation(direction);

        }

        public override void Attack(Character characterToAttack)
        {
            throw new System.NotImplementedException();
        }
    }
}
