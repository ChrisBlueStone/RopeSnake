using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RopeSnake.Mother3.IO;
using YamlDotNet.Serialization;

namespace RopeSnake.Mother3
{
    public class RomSettings
    {
        public Mother3Version Version { get; set; }
        public Dictionary<string, TableInfo> DataTables { get; set; } = new Dictionary<string, TableInfo>();
        public Dictionary<string, int> BankAddresses { get; set; } = new Dictionary<string, int>();

        public static RomSettings FromYaml(string yamlPath)
        {
            using (var reader = File.OpenText(yamlPath))
            {
                Deserializer deserializer = new Deserializer();
                return deserializer.Deserialize<RomSettings>(reader);
            }
        }

        public void WriteYaml(string yamlPath)
        {
            using (var writer = File.CreateText(yamlPath))
            {
                Serializer serializer = new Serializer();
                serializer.Serialize(writer, this);
            }
        }
    }
}
