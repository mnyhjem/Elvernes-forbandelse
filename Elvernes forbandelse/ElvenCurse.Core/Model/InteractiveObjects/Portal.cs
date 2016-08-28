﻿using System.Linq;

namespace ElvenCurse.Core.Model.InteractiveObjects
{
    public class Portal:InteractiveObject
    {
        public Location Destination
        {
            get
            {
                if (_destination == null)
                {
                    var d = Parameters.FirstOrDefault(a => a.Key == "Destination");
                    if (d != null)
                    {
                        var dSpl = d.Value.Split(',');
                        _destination = new Location
                        {
                            WorldsectionId = int.Parse(dSpl[0]),
                            Y = int.Parse(dSpl[1]),
                            X = int.Parse(dSpl[2])
                        };
                    }
                }

                return _destination;
            }
        }

        private Location _destination;

        public override InteractiveobjectResult Interact(Character character)
        {
            // man må maks være én tile fra portalen for den kan virke..

            // Send brugeren afsted..
            character.Location.WorldsectionId = Destination.WorldsectionId;
            character.Location.Y = Destination.Y;
            character.Location.X = Destination.X;

            return InteractiveobjectResult.ChangeUsersMap;
        }
    }
}
