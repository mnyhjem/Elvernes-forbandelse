using System;
using System.Collections.Generic;
using System.Linq;

namespace ElvenCurse.Core.Model.Creatures
{
    public abstract class Creature
    {
        protected Random Rnd { get; set; }

        public int Id { get; set; }
        
        public string Name { get; set; }

        public Location DefaultLocation { get; set; }

        public Location CurrentLocation { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public Creaturestatus Status { get; set; }

        public Creaturemode Mode { get; set; }

        private readonly int _viewDistace;
        protected readonly int AttackDistance;
        private readonly int _maxDistanceFromDefault = 25;

        public CreatureAction Action { get; set; }

        public bool UpdateNeeded
        {
            get
            {
                var r = _updateNeeded;
                _updateNeeded = false;
                return r;
            }
        }
        private bool _updateNeeded;

        public Character LastCharacterAttacked { get; set; }

        protected Creature(int viewDistance, int attackDistance)
        {
            _viewDistace = viewDistance;
            AttackDistance = attackDistance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterToAttack"></param>
        /// <returns>True if we did attack someone</returns>
        public abstract bool Attack(Character characterToAttack);

        public bool IsWithinDistance(Location location1, Location location2, int distance)
        {
            return !(CurrentLocation.X > DefaultLocation.X + _maxDistanceFromDefault ||
                    CurrentLocation.X < DefaultLocation.X - _maxDistanceFromDefault ||
                    CurrentLocation.Y > DefaultLocation.Y + _maxDistanceFromDefault ||
                    CurrentLocation.Y < DefaultLocation.Y - _maxDistanceFromDefault);
        }

        public virtual void CalculateNextMove(List<Character> characters, CreatureMovetype movetype)
        {
            // Se om vi er for langt væk fra vores "hjem"
            if (Action == CreatureAction.ReturnToDefaultLocation || !IsWithinDistance(CurrentLocation, DefaultLocation, _maxDistanceFromDefault))
            {
                Action = CreatureAction.ReturnToDefaultLocation;
                MoveTowardsLocation(DefaultLocation);
                return;
            }

            var collidesWith = CollidesViewPlayer(characters);
            if (collidesWith != null || Action == CreatureAction.Attacking)
            {
                if (Attack(collidesWith))
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

            var direction = directions[rnd.Next(directions.Count)];
            MoveTowardsLocation(direction);

        }

        protected void MoveTowardsLocation(Location location)
        {
            if (location.X < CurrentLocation.X)
            {
                CurrentLocation.X--;
                _updateNeeded = true;
            }
            else if (location.X > CurrentLocation.X)
            {
                CurrentLocation.X++;
                _updateNeeded = true;
            }
            else if (location.Y < CurrentLocation.Y)
            {
                CurrentLocation.Y--;
                _updateNeeded = true;
            }
            else if (location.Y > CurrentLocation.Y)
            {
                CurrentLocation.Y++;
                _updateNeeded = true;
            }

            if (!_updateNeeded)
            {
                Action = CreatureAction.Idle;
            }
        }

        protected Character CollidesViewPlayer(List<Character> characters)
        {
            return characters.FirstOrDefault(a => (a.Location.WorldsectionId == CurrentLocation.WorldsectionId) &&
                a.Location.X >= CurrentLocation.X - _viewDistace &&
                a.Location.X <= CurrentLocation.X + _viewDistace &&
                a.Location.Y >= CurrentLocation.Y - _viewDistace &&
                a.Location.Y <= CurrentLocation.Y + _viewDistace);
        }
    }
}
