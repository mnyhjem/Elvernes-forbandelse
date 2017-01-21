using System.Collections.Generic;

namespace ElvenCurse.Core.Model.Creatures.Npcs
{
    public abstract class NpcBase : Creature
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public Npcrace Race { get; set; }

        

        public CreatureMovetype Movetype { get; set; }

        protected NpcBase(int viewDistance, int attackDistance, Creaturetype type) : base(type, viewDistance, attackDistance)
        {
        }

        public void Move(List<Character> characters)
        {
            CalculateNextMove(characters, Movetype);
        }

        public override bool Attack(Creature characterToAttack, int activatedAbility)
        {
            return false;
        }

        public dynamic ToIPlayer()
        {
            return new
            {
                Id,
                Name,
                Location = Location,
                Type = Type,
                MaxHealth = GetMaxHealth(),
                Health = Health,
                IsAlive = IsAlive
            };
        }
    }
}
