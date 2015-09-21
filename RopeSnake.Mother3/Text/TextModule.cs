using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.Project;
using RopeSnake.Mother3.IO;

namespace RopeSnake.Mother3.Text
{
    public class TextModule : IModule
    {
        [ModuleFile("item-names")]
        public List<string> ItemNames { get; set; }

        public TextModule(Mother3Rom rom)
        {
            Mother3Reader reader = new Mother3Reader(rom);

            reader.Position = rom.Settings.BankAddresses["TextTable"];
            int[] pointers = reader.ReadOffsetTable();

            ReadItemNames(rom, pointers[2]);
        }

        private void ReadItemNames(Mother3Rom rom, int offset)
        {
            Mother3Reader reader = new Mother3Reader(rom);
            reader.Position = offset;

            FixedTableHeader header = reader.ReadFixedTableHeader();
            ItemNames = new List<string>();

            for (int i = 0; i < header.Count; i++)
            {
                ItemNames.Add(reader.ReadCodedString(header.EntryLength));
            }
        }

        public void ReadModule(string projectFolder)
        {
            ProjectHelpers.ReadJsonFiles(projectFolder, "text", this);
        }

        public void WriteModule(string projectFolder)
        {
            ProjectHelpers.WriteJsonFiles(projectFolder, "text", this);
        }
    }
}
