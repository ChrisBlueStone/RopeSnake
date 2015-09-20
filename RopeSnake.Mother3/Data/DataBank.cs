using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.Mother3.IO;

namespace RopeSnake.Mother3.Data
{
    public class DataBank
    {
        private Mother3Rom rom;

        public List<Item> Items { get; set; } = new List<Item>();

        public DataBank(Mother3Rom rom)
        {
            this.rom = rom;

            ReadItems();
        }

        private void ReadItems()
        {
            IMother3Reader reader = new Mother3Reader(rom);

            TableInfo info = rom.Settings.DataTables["Items"];
            reader.Position = info.Address;

            for (int i = 0; i < info.Count; i++)
                Items.Add(reader.ReadItem());
        }
    }
}
