using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RopeSnake.Mother3.IO;
using Newtonsoft.Json;

namespace RopeSnake.Mother3
{
    public class RomSettings
    {
        public Mother3Version Version { get; set; }
        public Dictionary<string, TableInfo> DataTables { get; set; }
        public Dictionary<string, int> BankAddresses { get; set; }
        public Dictionary<short, string> CharLookup { get; set; }

        public RomSettings(string jsonPath)
        {
            using (var reader = File.OpenText(jsonPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Populate(reader, this);
            }
        }
    }
}
