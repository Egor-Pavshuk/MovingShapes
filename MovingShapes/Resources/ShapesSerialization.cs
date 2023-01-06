using MovingShapes.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace MovingShapes.Resources
{
    internal class ShapesSerialization
    {
        private static string _filePath = "../../../SavedFiles/Shapes";

        public ShapesSerialization()
        {
            _filePath = "../../../SavedFiles/Shapes";
        }

        public ShapesSerialization(string filePath)
        {
            _filePath = filePath;
        }

        public static void SerializeToBin(List<CustomShape> customShapes)
        {
            using (Stream SaveFileStream = File.Open(_filePath + ".bin", FileMode.OpenOrCreate))
            {
                BinaryFormatter serializer = new BinaryFormatter();

                serializer.Serialize(SaveFileStream, customShapes);

                SaveFileStream.Flush();
                SaveFileStream.Close();
            }
        }

        public static List<CustomShape> DeserializeFromBin()
        {
            if (!File.Exists(_filePath + ".bin"))
            {
                return new List<CustomShape>();
            }

            using (Stream SaveFileStream = File.Open(_filePath + ".bin", FileMode.OpenOrCreate))
            {
                BinaryFormatter serializer = new BinaryFormatter();

                var shapes = (List<CustomShape>)serializer.Deserialize(SaveFileStream);

                SaveFileStream.Flush();
                SaveFileStream.Close();

                return shapes;
            }
        }

        public static void SerializeToXml(List<CustomShape> customShapes)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<CustomShape>), new Type[] { typeof(CustomCircle), typeof(CustomSquare), typeof(CustomTriangle) });
            using (var writer = new StreamWriter(_filePath + ".xml"))
            {
                xmlSerializer.Serialize(writer, customShapes);
            }
        }

        public static List<CustomShape> DeserializeFromXml()
        {
            if (!File.Exists(_filePath + ".xml"))
            {
                return new List<CustomShape>();
            }

            var xmlSerializer = new XmlSerializer(typeof(List<CustomShape>), new Type[] { typeof(CustomCircle), typeof(CustomSquare), typeof(CustomTriangle) });
            using (var reader = new StreamReader(_filePath + ".xml"))
            {
                var result = xmlSerializer.Deserialize(reader);
                if (result is null)
                {
                    return new List<CustomShape>();
                }
                var shapes = (List<CustomShape>)result;

                return shapes;
            }
        }

        public static void SerializeToJson(List<CustomShape> customShapes)
        {
            using (StreamWriter FileStreamWriter = new StreamWriter(_filePath + ".json"))
            {
                JsonSerializer jsonSerializer = new();
                jsonSerializer.TypeNameHandling = TypeNameHandling.Auto;
                using (JsonWriter writer = new JsonTextWriter(FileStreamWriter))
                {
                    jsonSerializer.Serialize(writer, customShapes, typeof(List<CustomShape>));
                }
            }
        }

        public static List<CustomShape> DeserializeFromJson()
        {
            if (!File.Exists(_filePath + ".json"))
            {
                return new List<CustomShape>();
            }

            using (StreamReader FileStreamReader = new StreamReader(_filePath + ".json"))
            {
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                var result = JsonConvert.DeserializeObject(FileStreamReader.ReadToEnd(), typeof(List<CustomShape>), settings);
                if (result is null)
                {
                    return new List<CustomShape>();
                }
                var shapes = (List<CustomShape>)result;
                return shapes;
            }
        }
    }
}
