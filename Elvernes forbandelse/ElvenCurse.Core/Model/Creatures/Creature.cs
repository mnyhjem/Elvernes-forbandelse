using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ElvenCurse.Core.Utilities;

namespace ElvenCurse.Core.Model.Creatures
{
    public abstract class Creature
    {
        protected Random Rnd { get; set; }

        public int Id { get; set; }
        
        public string Name { get; set; }

        public Location DefaultLocation { get; set; }

        //public Location CurrentLocation { get; set; }

        public Location Location { get; set; }

        public Creaturetype Type { get; private set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public bool IsAlive
        {
            get { return _health > 0; }
        }

        public Creaturemode Mode { get; set; }

        public virtual int Level { get; set; }

        public int Basehealth
        {
            get
            {
                return _baseHealth;
            }
            set
            {
                _baseHealth = value;
                ResetHealth();
            }
        }

        private int _baseHealth;

        public virtual int Health
        {
            get { return _health < 0 ? 0 : _health; }
        }

        private int _health;

        public int MaxHealth
        {
            get
            {
                return GetMaxHealth();
            }
        }

        public List<CreatureAbility> Abilities { get; set; }
        public Stack<AffectedByAbility> AffectedByAbilities { get; set; }

        private readonly int _viewDistace;
        protected readonly int AttackDistance;
        private readonly int _maxDistanceFromDefault = 25;

        public CreatureAction Action { get; set; }

        public bool UpdateNeeded { get; set; }

        public Creature LastCreatureAttacked { get; set; }
        public CharacterAppearance CharacterAppearance { get; set; }

        protected Creature(Creaturetype type, int viewDistance, int attackDistance)
        {
            _viewDistace = viewDistance;
            AttackDistance = attackDistance;
            Type = type;
            Abilities = new List<CreatureAbility>();
            AffectedByAbilities = new Stack<AffectedByAbility>();

            Abilities.Add(new CreatureAbility(this)
            {
                BaseHeal = 5,
                Cooldown = 2,
                Name = "Selvhealing",
                Passive = true,
                IsHeal = true
            });

            GetAbilitiesForCreature();
        }

        private void GetAbilitiesForCreature()
        {
            switch (Type)
            {
                case Creaturetype.Hunter:
                    Abilities.Add(new CreatureAbility(this)
                    {
                        Cooldown = 2,
                        Name = "Skyd",
                        BaseDamage = 10,
                        AbilityIcon = 72,
                        AttackDistance = 10
                    });
                    break;
            }
        }

        protected int GetMaxHealth()
        {
            return _baseHealth + (15 * Level) - 15;
        }

        public void ResetHealth()
        {
            _health = GetMaxHealth();
        }

        public void SetHealth(int health)
        {
            _health = health;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterToAttack"></param>
        /// <param name="activatedAbility"></param>
        /// <returns>True if we did attack someone</returns>
        public abstract bool Attack(Creature characterToAttack, int activatedAbility);

        //public bool IsWithinDistance(Location location1, Location location2, int distance)
        //{


        //    //return !(CurrentLocation.X > DefaultLocation.X + _maxDistanceFromDefault ||
        //    //        CurrentLocation.X < DefaultLocation.X - _maxDistanceFromDefault ||
        //    //        CurrentLocation.Y > DefaultLocation.Y + _maxDistanceFromDefault ||
        //    //        CurrentLocation.Y < DefaultLocation.Y - _maxDistanceFromDefault);
        //}

        public void ProcessAffectedby()
        {
            if (AffectedByAbilities.Count > 0)
            {
                UpdateNeeded = true;
            }

            while (AffectedByAbilities.Count > 0)
            {
                var a = AffectedByAbilities.Pop();
                _health += a.Healtheffect;

                if (_health <= 0)
                {
                    Trace.WriteLine(string.Format("{0} døede", Name));
                }

                if (_health > MaxHealth)
                {
                    _health = MaxHealth;
                }
            }
        }

        public virtual void CalculateNextMove(List<Character> characters, CreatureMovetype movetype)
        {
            if (!IsAlive)
            {
                return;
            }

            // Se om vi er for langt væk fra vores "hjem"
            if (Action == CreatureAction.ReturnToDefaultLocation || !Location.IsWithinReachOf(DefaultLocation, _maxDistanceFromDefault))
            {
                Action = CreatureAction.ReturnToDefaultLocation;
                MoveTowardsLocation(DefaultLocation);
                return;
            }

            var collidesWith = CollidesViewPlayer(characters);
            if (collidesWith != null || Action == CreatureAction.Attacking)
            {
                if (Attack(collidesWith, -1))
                {
                    return;
                }
            }

            switch (movetype)
            {
                case CreatureMovetype.FollowCharactor:
                    FollowCharactor(characters);
                    break;

                case CreatureMovetype.Random:
                    MoveRandomly(Rnd);
                    break;
            }
        }

        private void FollowCharactor(List<Character> characters)
        {
            // Se om vi skal løbe efter en spiller
            var collisionWithPlayer = CollidesViewPlayer(characters);
            if (collisionWithPlayer != null)
            {
                Action = CreatureAction.FollowPlayer;
                MoveTowardsLocation(collisionWithPlayer.Location);
            }
        }

        protected void MoveRandomly(Random rnd)
        {
            var directions = new List<Location>();
            directions.Add(DefaultLocation);

            if (Location.X > DefaultLocation.X - 20)
            {
                directions.Add(new Location
                {
                    WorldsectionId = DefaultLocation.WorldsectionId,
                    X = DefaultLocation.X - 20,
                    Y = DefaultLocation.Y
                });
            }
            if (Location.X < DefaultLocation.X + 20)
            {
                directions.Add(new Location
                {
                    WorldsectionId = DefaultLocation.WorldsectionId,
                    X = DefaultLocation.X + 20,
                    Y = DefaultLocation.Y
                });
            }
            if (Location.Y > DefaultLocation.Y - 20)
            {
                directions.Add(new Location
                {
                    WorldsectionId = DefaultLocation.WorldsectionId,
                    X = DefaultLocation.X,
                    Y = DefaultLocation.Y - 20
                });
            }
            if (Location.Y < DefaultLocation.Y + 20)
            {
                directions.Add(new Location
                {
                    WorldsectionId = DefaultLocation.WorldsectionId,
                    X = DefaultLocation.X,
                    Y = DefaultLocation.Y + 20
                });
            }

            var direction = directions[rnd.Next(directions.Count)];
            MoveTowardsLocation(direction);

        }

        protected void MoveTowardsLocation(Location location)
        {
            if (location.X < Location.X)
            {
                Location.X--;
                UpdateNeeded = true;
            }
            else if (location.X > Location.X)
            {
                Location.X++;
                UpdateNeeded = true;
            }
            else if (location.Y < Location.Y)
            {
                Location.Y--;
                UpdateNeeded = true;
            }
            else if (location.Y > Location.Y)
            {
                Location.Y++;
                UpdateNeeded = true;
            }

            if (!UpdateNeeded)
            {
                Action = CreatureAction.Idle;
            }
        }

        protected Character CollidesViewPlayer(List<Character> characters)
        {
            return characters.FirstOrDefault(a => (a.Location.WorldsectionId == Location.WorldsectionId) && a.IsAlive && a.Location.IsWithinReachOf(Location, _viewDistace));
            //return characters.FirstOrDefault(a => (a.Location.WorldsectionId == CurrentLocation.WorldsectionId) &&
            //    a.Location.X >= CurrentLocation.X - _viewDistace &&
            //    a.Location.X <= CurrentLocation.X + _viewDistace &&
            //    a.Location.Y >= CurrentLocation.Y - _viewDistace &&
            //    a.Location.Y <= CurrentLocation.Y + _viewDistace);
        }
    }
}
