using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RopeSnake.Mother3.IO
{
    public static class Helpers
    {
        public static void WriteDataBanks(string projectFolder, string subFolder, IDictionary<string, object> banks)
        {
            foreach (var fileMapping in banks)
            {
                string fileName = Path.Combine(projectFolder, subFolder, fileMapping.Key);

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
                    serializer.Serialize(writer, fileMapping.Value);
                }
            }
        }
    }
}
