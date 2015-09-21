using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RopeSnake.Project
{
    public static class ProjectHelpers
    {
        public static void ReadJsonFiles(string projectFolder, string subFolder, object destination)
        {
            Type bankType = destination.GetType();
            PropertyInfo[] properties = bankType.GetProperties();

            foreach (var property in properties)
            {
                var attributes = property.CustomAttributes.Where(a => a.AttributeType == typeof(ModuleFileAttribute));

                if (attributes.Count() > 1)
                {
                    throw new Exception($"Only allowed one JsonFile attribute per property: {property.Name} in {bankType.Name}");
                }

                CustomAttributeData attribute = attributes.FirstOrDefault();

                if (attribute != null)
                {
                    string jsonFileName = (string)attribute.ConstructorArguments[0].Value;
                    string fileName = Path.Combine(projectFolder, subFolder, jsonFileName + ".json");
                    object data;

                    using (var reader = File.OpenText(fileName))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        data = serializer.Deserialize(reader, property.PropertyType);
                    }

                    property.SetValue(destination, data);
                }
            }
        }

        public static void WriteJsonFiles(string projectFolder, string subFolder, object source)
        {
            Type bankType = source.GetType();
            PropertyInfo[] properties = bankType.GetProperties();

            foreach (var property in properties)
            {
                var attributes = property.CustomAttributes.Where(a => a.AttributeType == typeof(ModuleFileAttribute));

                if (attributes.Count() > 1)
                {
                    throw new Exception($"Only allowed one JsonFile attribute per property: {property.Name} in {bankType.Name}");
                }

                CustomAttributeData attribute = attributes.FirstOrDefault();

                if (attribute != null)
                {
                    string jsonFileName = (string)attribute.ConstructorArguments[0].Value;
                    string fileName = Path.Combine(projectFolder, subFolder, jsonFileName + ".json");
                    object data = property.GetValue(source);

                    FileInfo file = new FileInfo(fileName);

                    if (!file.Directory.Exists)
                    {
                        file.Directory.Create();
                    }

                    using (var writer = File.CreateText(fileName))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Converters.Add(new StringEnumConverter());
                        serializer.Converters.Add(new ByteArrayConverter());
                        serializer.Formatting = Formatting.Indented;
                        serializer.Serialize(writer, data);
                    }
                }
            }
        }
    }
}
