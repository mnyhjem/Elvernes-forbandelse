using System.Linq;
using ElvenCurse.Core.Utilities;
using Microsoft.Owin.Security.DataProtection;

namespace ElvenCurse.Core.Model.InteractiveObjects
{
    public class Portal:InteractiveObject
    {
        private Location Destination
        {
            get
            {
                if (_destination == null)
                {
                    var d = Parameters.FirstOrDefault(a => a.Key == "Destination");
                    if (d != null)
                    {
                        var dest = new Location();

                        var dSpl = d.Value.Split(',');
                        foreach (var p in dSpl)
                        {
                            var parameter = p.Split(':');
                            switch (parameter[0].ToLower())
                            {
                                case "worldsectionid":
                                    dest.WorldsectionId = int.Parse(parameter[1]);
                                    break;
                                case "x":
                                    dest.X = int.Parse(parameter[1]);
                                    break;
                                case "y":
                                    dest.Y = int.Parse(parameter[1]);
                                    break;
                            }
                        }
                        _destination = dest;
                    }
                }

                return _destination;
            }
        }

        private Location _destination;

        public override InteractiveobjectResult Interact(Character character)
        {
            // man må maks være én tile fra portalen for den kan virke..
            if (!character.Location.IsWithinReachOf(Location, 1))
            {
                return InteractiveobjectResult.NoChange;
            }

            // Send brugeren afsted..
            character.Location.WorldsectionId = Destination.WorldsectionId;
            character.Location.Y = Destination.Y;
            character.Location.X = Destination.X;

            return InteractiveobjectResult.ChangeUsersMap;
        }
    }
}
