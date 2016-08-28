using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ElvenCurse.Core.Model;

namespace ElvenCurse.Core.Utilities
{
    public static class ExtensionsAndUtilities
    {
        public static T DeepCopy<T>(T obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                return (T) formatter.Deserialize(stream);
            }
        }

        public static bool IsWithinReachOf(this Location fromLocation, Location targetLocation, int maxDistance)
        {
            if (Math.Abs(targetLocation.X - fromLocation.X) <= maxDistance &&
                Math.Abs(targetLocation.Y - fromLocation.Y) <= maxDistance)
            {
                return true;
            }
            
            return false;
        }
    }
}
