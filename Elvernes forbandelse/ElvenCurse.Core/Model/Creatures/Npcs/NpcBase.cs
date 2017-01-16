using System.Collections.Generic;

namespace ElvenCurse.Core.Model.Creatures.Npcs
{
    public abstract class NpcBase : Creature
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public Npcrace Race { get; set; }

        public Npctype Type { get; set; }

        public CreatureMovetype Movetype { get; set; }

        protected NpcBase(int viewDistance, int attackDistance, Npctype type) : base(viewDistance, attackDistance)
        {
            Type = type;
        }

        public void Move(List<Character> characters)
        {
            CalculateNextMove(characters, Movetype);
        }

        public override bool Attack(Character characterToAttack)
        {
            return false;
        }

        public dynamic ToIPlayer()
        {
            return new
            {
                Id,
                Name,
                Location = CurrentLocation,
                Type = Type,
                MaxHealth = GetMaxHealth(),
                Health = Health,
                IsAlive = IsAlive
            };
        }
    }
}
