using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
    }
}
