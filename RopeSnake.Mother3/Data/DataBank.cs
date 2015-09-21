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
        private Dictionary<string, object> fileMap = new Dictionary<string, object>();
        public List<Item> Items { get; set; } = new List<Item>();

        public DataBank(Mother3Rom rom)
        {
            ReadItems(rom);
            PopulateFileMap();
        }

        private void PopulateFileMap()
        {
            fileMap.Add("items.json", Items);
        }

        private void ReadItems(Mother3Rom rom)
        {
            IMother3Reader reader = new Mother3Reader(rom);

            TableInfo info = rom.Settings.DataTables["Items"];
            reader.Position = info.Address;

            for (int i = 0; i < info.Count; i++)
                Items.Add(reader.ReadItem());
        }

        public void WriteJson(string projectDirectory)
        {
            Helpers.WriteDataBanks(projectDirectory, "data", fileMap);
        }
    }
}
