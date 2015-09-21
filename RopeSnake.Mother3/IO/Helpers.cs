using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RopeSnake.Mother3.IO
{
    public static class Helpers
    {
        public static void WriteDataBanks(string projectFolder, string subFolder, object bankCollection)
        {
            Type bankType = bankCollection.GetType();
            PropertyInfo[] properties = bankType.GetProperties();

            foreach (var property in properties)
            {
                var attributes = property.CustomAttributes.Where(a => a.AttributeType == typeof(JsonFileAttribute));

                if (attributes.Count() > 1)
                {
                    throw new Exception($"Only allowed one JsonFile attribute per property: {property.Name} in {bankType.Name}");
                }

                CustomAttributeData attribute = attributes.FirstOrDefault();

                if (attribute != null)
                {
                    string jsonFileName = (string)attribute.ConstructorArguments[0].Value;
                    string fileName = Path.Combine(projectFolder, subFolder, jsonFileName);
                    object data = property.GetValue(bankCollection);

                    FileInfo file = new FileInfo(fileName);

                    if (!file.Directory.Exists)
                    {
                        file.Directory.Create();
                    }

                    using (var writer = File.CreateText(fileName))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Converters.Add(new StringEnumConverter());
                        serializer.Formatting = Formatting.Indented;
                        serializer.Serialize(writer, data);
                    }
                }
            }
        }
    }
}
