using System.Collections.Generic;
using System.Linq;

namespace ElvenCurse.Core.Model
{
    public abstract class Npc
    {
        public int Id { get; set; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public Npcrace Race { get; set; }
        public string Name { get; set; }
        public Location DefaultLocation { get; set; }
        public Location CurrentLocation { get; set; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public Npcstatus Status { get; set; }

        private readonly int _viewDistace;
        private readonly int _attachDistance;
        private readonly int _maxDistanceFromDefault = 25;

        public NpcAction Action { get; set; }

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

        protected Npc(int viewDistance, int attachDistance)
        {
            _viewDistace = viewDistance;
            _attachDistance = attachDistance;
        }

        public abstract void Attack(Character characterToAttack);

        public void CalculateNextMove(List<Character> characters)
        {
            // Se om vi er for langt væk fra vores "hjem"
            if (Action == NpcAction.ReturnToDefaultLocation ||
                (CurrentLocation.X > DefaultLocation.X + _maxDistanceFromDefault ||
                CurrentLocation.X < DefaultLocation.X - _maxDistanceFromDefault ||
                CurrentLocation.Y > DefaultLocation.Y + _maxDistanceFromDefault ||
                CurrentLocation.Y < DefaultLocation.Y - _maxDistanceFromDefault))
            {
                Action = NpcAction.ReturnToDefaultLocation;
                MoveTowardsLocation(DefaultLocation);
                return;
            }

            // Se om vi skal løbe efter en spiller
            var collisionWithPlayer = CollidesViewPlayer(characters);
            if (collisionWithPlayer != null)
            {
                Action = NpcAction.FollowPlayer;
                MoveTowardsLocation(collisionWithPlayer.Location);
            }
        }

        private void MoveTowardsLocation(Location location)
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
                Action = NpcAction.Idle;
            }
        }

        private Character CollidesViewPlayer(List<Character> characters)
        {
            return characters.FirstOrDefault(a => (a.Location.WorldsectionId == CurrentLocation.WorldsectionId) &&
                a.Location.X >= CurrentLocation.X - _viewDistace &&
                a.Location.X <= CurrentLocation.X + _viewDistace &&
                a.Location.Y >= CurrentLocation.Y - _viewDistace &&
                a.Location.Y <= CurrentLocation.Y + _viewDistace);
        }


        public dynamic ToIPlayer()
        {
            return new
            {
                Id,
                Name,
                Location = CurrentLocation
            };
        }
    }
}
