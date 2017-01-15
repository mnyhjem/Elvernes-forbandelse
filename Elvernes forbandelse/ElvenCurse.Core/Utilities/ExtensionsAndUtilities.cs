using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using ElvenCurse.Core.Model;

namespace ElvenCurse.Core.Utilities
{
    public static class ExtensionsAndUtilities
    {
        public static string GetDisplayname(this Enum enumValue)
        {
            try
            {
                var attr = enumValue.GetType().GetMember(enumValue.ToString()).First().GetCustomAttribute<DisplayAttribute>();

                //Der checkes på om resourceType er null, da getproperty fejlede i situationer, hvor dette var null
                var resourname = "";
                if (attr.ResourceType != null)
                {
                    resourname = attr.ResourceType.GetProperty(attr.Name).GetValue(null, null) as string;
                }

                if (!string.IsNullOrWhiteSpace(resourname))
                {
                    return resourname;
                }
                return attr.Name;
            }
            catch (TypeLoadException)
            {
            }
            catch (InvalidOperationException)
            {// denne kommer hvis enumværdien mangler
            }
            catch (AmbiguousMatchException)
            {
            }
            catch (NullReferenceException)
            {
                // Denne kommer hvis der ikke er en [Display] halløj på vores enum.. vi returnere vi bare værdien uændret :)
                return enumValue.ToString();
            }
            return "Intet navn";
        }

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
            var res = GetSquaredDistance(fromLocation, targetLocation);

            return res < maxDistance * maxDistance;


            //    if (Math.Abs(targetLocation.X - fromLocation.X) <= maxDistance &&
            //        Math.Abs(targetLocation.Y - fromLocation.Y) <= maxDistance)
            //    {
            //        return true;
            //    }

            //    return false;
        }

        public static int GetDistanceBetweenLocations(this Location fromLocation, Location targetLocation)
        {
            var res = GetSquaredDistance(fromLocation, targetLocation);

            return (int)Math.Sqrt(res);
        }

        private static int GetSquaredDistance(Location fromLocation, Location targetLocation)
        {
            var res = ((fromLocation.X - targetLocation.X) * (fromLocation.X - targetLocation.X) + (fromLocation.Y - targetLocation.Y) * (fromLocation.Y - targetLocation.Y));

            return res;
        }

        public static T ParseXML<T>(this string @this) where T : class
        {
            var reader = XmlReader.Create(@this.Trim().ToStream(), 
                new XmlReaderSettings()
                {
                    ConformanceLevel = ConformanceLevel.Document
                });
            return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
        }

        public static Stream ToStream(this string @this)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(@this);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
