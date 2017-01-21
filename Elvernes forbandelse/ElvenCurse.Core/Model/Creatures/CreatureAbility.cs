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
        public int BaseHeal { get; set; }
        public bool Passive { get; set; }
        public bool IsHeal { get; set; }

        private DateTime _lastUsed;

        public CreatureAbility(Creature owner)
        {
            Owner = owner;
        }

        private int DamageCalculation(int characterLevel, int ownerLevel)
        {
            //var damage = Owner.Level - characterLevel + 1 * BaseDamage * Owner.Level;
            var damage = ownerLevel * BaseDamage + ((ownerLevel - characterLevel) * BaseDamage) * 2;
            
            // hvis damage er negativ, ville vi heale.. og det skal vi ikke. i stedet giver vi ingen skade..
            if (damage < 0)
            {
                damage = 0;
            }
            return damage;
        }

        private int HealCalculation(int characterLevel, int ownerLevel)
        {
            return BaseHeal;
        }

        public void Use(Creature characterToAffect)
        {
            string ownerName;
            int ownerLevel;
            if (Passive)
            {
                ownerName = characterToAffect.Name;
                ownerLevel = characterToAffect.Level;
            }
            else
            {
                ownerName = Owner.Name;
                ownerLevel = Owner.Level;
            }

            if ((DateTime.Now - _lastUsed).TotalSeconds < Cooldown)
            {
                return;
            }

            _lastUsed = DateTime.Now;
            int healthEffect;
            if (IsHeal)
            {
                if (characterToAffect.Health == characterToAffect.MaxHealth || !characterToAffect.IsAlive)
                {
                    return;
                }
                healthEffect = HealCalculation(characterToAffect.Level, ownerLevel);
                Trace.WriteLine(string.Format("{0} bruger {1} på {2} for {3} healing [liv {4}/{5}]", ownerName, Name, characterToAffect.Name, healthEffect, characterToAffect.Health, characterToAffect.MaxHealth));
            }
            else
            {
                healthEffect = -DamageCalculation(characterToAffect.Level, ownerLevel);
                Trace.WriteLine(string.Format("{0} bruger {1} på {2} for {3} skade [liv {4}/{5}]", ownerName, Name, characterToAffect.Name, healthEffect, characterToAffect.Health, characterToAffect.MaxHealth));
            }
            
            characterToAffect.AffectedByAbilities.Push(new AffectedByAbility
            {
                DoneBy = Owner,
                Name = Name,
                Healtheffect = healthEffect
            });
        }
    }
}
