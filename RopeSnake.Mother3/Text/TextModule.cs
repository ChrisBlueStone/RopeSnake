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
        [ModuleFile("room-descriptions")]
        public Dictionary<int, string> RoomDescriptions { get; set; }

        [ModuleFile("item-names")]
        public Dictionary<int, string> ItemNames { get; set; }

        [ModuleFile("item-descriptions")]
        public Dictionary<int, string> ItemDescriptions { get; set; }

        [ModuleFile("character-names")]
        public Dictionary<int, string> CharacterNames { get; set; }

        [ModuleFile("party-character-names")]
        public Dictionary<int, string> PartyCharacterNames { get; set; }

        [ModuleFile("enemy-names")]
        public Dictionary<int, string> EnemyNames { get; set; }

        [ModuleFile("psi-names")]
        public Dictionary<int, string> PsiNames { get; set; }

        [ModuleFile("psi-descriptions")]
        public Dictionary<int, string> PsiDescriptions { get; set; }

        [ModuleFile("statuses")]
        public Dictionary<int, string> Statuses { get; set; }

        [ModuleFile("default-names")]
        public Dictionary<int, string> DefaultNames { get; set; }

        [ModuleFile("special-text")]
        public Dictionary<int, string> SpecialText { get; set; }

        [ModuleFile("skill-descriptions")]
        public Dictionary<int, string> SkillDescriptions { get; set; }

        [ModuleFile("main-script")]
        public Dictionary<int, Dictionary<int, string>> MainScript { get; set; }

        public TextModule(string projectFolder)
        {
            ReadModule(projectFolder);
        }

        public TextModule(Mother3Rom rom)
        {
            // Text bank
            ReadTextBank(rom);

            // Main script
            ReadMainScript(rom);
        }

        private Dictionary<int, string> ReadOffsetText(Mother3Rom rom, int tableOffset, int textOffset, bool dialog)
        {
            Mother3Reader reader = new Mother3Reader(rom);

            reader.Position = tableOffset;
            int[] pointers = reader.ReadMiniOffsetTable(textOffset);

            Dictionary<int, string> text = new Dictionary<int, string>();

            for (int i = 0; i < pointers.Length; i++)
            {
                reader.Position = pointers[i];

                if (pointers[i] != 0)
                {
                    if (dialog)
                    {
                        text.Add(i, reader.ReadDialogString());
                    }
                    else
                    {
                        text.Add(i, reader.ReadCodedString());
                    }
                }
            }

            return text;
        }

        private Dictionary<int, string> ReadTableText(Mother3Rom rom, int offset, int bugContext = 0)
        {
            if (offset == 0)
            {
                return null;
            }

            Mother3Reader reader = new Mother3Reader(rom);
            reader.Position = offset;

            FixedTableHeader header = reader.ReadFixedTableHeader();

            if (bugContext == 1)
            {
                // Bug in English versions 1.0 through 1.2 -- one of the entries is
                // missing from the table, so the header's entry count is too high by one
                switch (rom.Settings.Version)
                {
                    case Mother3Version.English10:
                    case Mother3Version.English11:
                    case Mother3Version.English12:
                        header = new FixedTableHeader(header.EntryLength, header.Count - 1);
                        break;
                }
            }

            Dictionary<int, string> text = new Dictionary<int, string>();

            for (int i = 0; i < header.Count; i++)
            {
                text.Add(i, reader.ReadCodedString(header.EntryLength));
            }

            return text;
        }

        private void ReadTextBank(Mother3Rom rom)
        {
            Mother3Reader reader = new Mother3Reader(rom);
            reader.Position = rom.Settings.BankAddresses["TextTable"];
            int[] pointers = reader.ReadOffsetTable();

            RoomDescriptions = ReadOffsetText(rom, pointers[0], pointers[1], false);
            ItemNames = ReadTableText(rom, pointers[2]);
            ItemDescriptions = ReadOffsetText(rom, pointers[3], pointers[4], false);
            CharacterNames = ReadTableText(rom, pointers[5]);
            PartyCharacterNames = ReadTableText(rom, pointers[6]);
            EnemyNames = ReadTableText(rom, pointers[7], bugContext: 1);
            PsiNames = ReadTableText(rom, pointers[8]);
            PsiDescriptions = ReadOffsetText(rom, pointers[9], pointers[10], false);
            Statuses = ReadTableText(rom, pointers[11]);
            DefaultNames = ReadTableText(rom, pointers[12]);
            SpecialText = ReadTableText(rom, pointers[13]);
            SkillDescriptions = ReadOffsetText(rom, pointers[14], pointers[15], false);
        }

        private void ReadMainScript(Mother3Rom rom)
        {
            // The main script is technically part of the map bank, but it makes more sense here
            Mother3Reader reader = new Mother3Reader(rom);
            reader.Position = rom.Settings.BankAddresses["Maps.MainScript"];
            int[] pointers = reader.ReadOffsetTable();

            MainScript = new Dictionary<int, Dictionary<int, string>>();

            int entryCount = pointers.Length / 2;
            for (int i = 0; i < entryCount; i++)
            {
                int miniOffsetPointer = pointers[i * 2];
                int textPointer = pointers[i * 2 + 1];

                if (miniOffsetPointer != 0 && textPointer != 0)
                {
                    MainScript.Add(i, ReadOffsetText(rom, miniOffsetPointer, textPointer, true));
                }
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
