using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RopeSnake.Mother3.IO;
using Newtonsoft.Json;

namespace RopeSnake.Mother3.Data
{
    public class DataBank
    {
        [JsonFile("items.json")]
        public List<Item> Items { get; set; }

        public DataBank(string projectDirectory)
        {
            ReadJson(projectDirectory);
        }

        public DataBank(Mother3Rom rom)
        {
            ReadItems(rom);
        }

        private void ReadItems(Mother3Rom rom)
        {
            IMother3Reader reader = new Mother3Reader(rom);

            TableInfo info = rom.Settings.DataTables["Items"];
            reader.Position = info.Address;

            Items = new List<Item>();
            for (int i = 0; i < info.Count; i++)
                Items.Add(reader.ReadItem());
        }

        private void ReadJson(string projectDirectory)
        {
            Helpers.ReadJsonFiles(projectDirectory, "data", this);
        }

        public void WriteJson(string projectDirectory)
        {
            Helpers.WriteJsonFiles(projectDirectory, "data", this);
        }
    }
}
