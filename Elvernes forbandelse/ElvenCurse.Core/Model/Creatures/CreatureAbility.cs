using System;
using System.Diagnostics;

namespace ElvenCurse.Core.Model.Creatures
{
    public class CreatureAbility
    {
        public Creature Owner { get; }

        public string Name { get; set; }
        public int Cooldown { get; set; }
        public int BaseDamage { get; set; }

        private DateTime _lastUsed;

        public CreatureAbility(Creature owner)
        {
            Owner = owner;
        }

        public int DamageCalculation(int characterLevel)
        {
            return Owner.Level - characterLevel + 1 * BaseDamage * Owner.Level;
        }

        public void Use(Character characterToAttack)
        {
            if ((DateTime.Now - _lastUsed).TotalSeconds < Cooldown)
            {
                return;
            }

            _lastUsed = DateTime.Now;

            var damageToDeal = DamageCalculation(characterToAttack.Level);

            Trace.WriteLine(string.Format("{0} bruger {1} på {2} for {3} skade", Owner.Name, Name, characterToAttack.Name, damageToDeal));

            characterToAttack.AffectedByAbilities.Push(new AffectedByAbility
            {
                DoneBy = Owner,
                Name = Name,
                Healtheffect = -damageToDeal
            });
        }
    }
}
