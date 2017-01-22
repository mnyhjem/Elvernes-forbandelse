using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ElvenCurse.Core.Model.Creatures.Npcs
{
    public abstract class NpcBase : Creature
    {
        private int _respawnTimeoutAfterDestroy;
        private int _destroyTimeout;
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public Npcrace Race { get; set; }

        public bool Destroy { get; private set; }

        public CreatureMovetype Movetype { get; set; }


        protected NpcBase(int viewDistance, int attackDistance, Creaturetype type) : base(type, viewDistance, attackDistance)
        {
            _destroyTimeout = 30;
            _respawnTimeoutAfterDestroy = 120;
        }

        public void CheckForDestroyAndRespawn()
        {
            if (!Destroy && !IsAlive && (DateTime.Now - TimeofDeath).TotalSeconds >= _destroyTimeout)
            {
                Trace.WriteLine($"Fjerner {Name}");
                UpdateNeeded = true;
                Destroy = true;
                return;
            }

            if (Destroy && !IsAlive && (DateTime.Now - TimeofDeath).TotalSeconds >= _destroyTimeout + _respawnTimeoutAfterDestroy)
            {
                Trace.WriteLine($"Genindsætter {Name}");
                Destroy = false;
                ResetHealth();
                Location = DefaultLocation;

                UpdateNeeded = true;
            }
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
                IsAlive = IsAlive,
                Connectionstatus = Destroy ? 0 : 1
            };
        }
    }
}
