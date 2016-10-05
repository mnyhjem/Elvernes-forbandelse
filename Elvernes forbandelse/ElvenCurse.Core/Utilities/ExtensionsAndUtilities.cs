﻿using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
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
