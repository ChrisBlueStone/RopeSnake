using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RopeSnake.Project;
using RopeSnake.Mother3.IO;
using Newtonsoft.Json;

namespace RopeSnake.Mother3.Data
{
    public class DataModule : IModule
    {
        [JsonFile("items.json")]
        public List<Item> Items { get; set; }

        public DataModule()
        {

        }

        public DataModule(string projectDirectory)
        {
            ReadJsonFiles(projectDirectory);
        }

        public DataModule(Mother3Rom rom)
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

        public void ReadJsonFiles(string projectDirectory)
        {
            ProjectHelpers.ReadJsonFiles(projectDirectory, "data", this);
        }

        public void WriteJsonFiles(string projectDirectory)
        {
            ProjectHelpers.WriteJsonFiles(projectDirectory, "data", this);
        }
    }
}
