using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.Mother3.Data;
using RopeSnake.Mother3.Text;

namespace RopeSnake.Mother3.IO
{
    public class Mother3RomReader : IMother3Data, IMother3Text
    {
        private IMother3Reader reader;
        private Mother3Rom rom;

        public Mother3RomReader(Mother3Rom rom)
        {
            this.rom = rom;

            StringReader stringReader;

            switch (rom.Settings.Version)
            {
                case Mother3Version.Japanese:
                case Mother3Version.None:
                case Mother3Version.Invalid:
                    stringReader = new JapaneseStringReader(rom);
                    break;

                case Mother3Version.English10:
                case Mother3Version.English11:
                case Mother3Version.English12:
                    stringReader = new EnglishStringReader(rom);
                    break;

                default:
                    throw new Exception("Unrecognized ROM version");
            }

            reader = new Mother3Reader(rom.Source, stringReader);
        }

        private int ReadPointerFromOffsetTable(int tableAddress, int offsetIndex)
        {
            reader.Position = tableAddress;
            int count = reader.ReadInt();

            if (offsetIndex >= count)
            {
                throw new Exception($"The offset index {offsetIndex} exceeds the entry count {count}");
            }

            reader.Position += (offsetIndex * 4);
            int offset = reader.ReadInt();

            if (offset == 0)
                return 0;

            return offset + tableAddress;
        }

        private int GetFixedTablePointer(int tableAddress, int index, out FixedTableHeader header)
        {
            reader.Position = tableAddress;
            header = reader.ReadFixedTableHeader();

            if (index >= header.Count)
            {
                throw new Exception($"The index {index} exceeds the entry count {header.Count}");
            }

            return reader.Position + (index * header.EntryLength * 2);
        }

        #region IMother3Data implementation

        public Item ReadItem(int index)
        {
            TableInfo tableInfo = rom.Settings.DataTables["Items"];
            reader.Position = tableInfo.Address + (index * tableInfo.EntryLength);

            Item item = new Item();

            item.Index = reader.ReadInt();
            item.Type = (ItemType)reader.ReadInt();
            item.Key = (reader.ReadByte() == 0);

            item.SetUnknown(0, reader.ReadByte());

            item.SellPrice = reader.ReadUShort();
            item.EquipFlags = (EquipFlags)reader.ReadUShort();

            for (int i = 0; i < 2; i++)
                item.SetUnknown(i + 1, reader.ReadByte());

            item.Hp = reader.ReadInt();
            item.Pp = reader.ReadShort();

            for (int i = 0; i < 2; i++)
                item.SetUnknown(i + 3, reader.ReadByte());

            item.Offense = reader.ReadSByte();
            item.Defense = reader.ReadSByte();
            item.Iq = reader.ReadSByte();
            item.Speed = reader.ReadSByte();

            for (int i = 0; i < 4; i++)
                item.SetUnknown(i + 5, reader.ReadByte());

            for (int i = 0; i < 11; i++)
                item.AilmentProtection[(AilmentType)i] = reader.ReadShort();

            for (int i = 0; i < 5; i++)
                item.ElementalProtection[(ElementalType)i] = reader.ReadSByte();

            for (int i = 0; i < 19; i++)
                item.SetUnknown(i + 9, reader.ReadByte());

            item.LowerHp = reader.ReadUShort();
            item.UpperHp = reader.ReadUShort();

            for (int i = 0; i < 10; i++)
                item.SetUnknown(i + 28, reader.ReadByte());

            item.BattleTextIndex = reader.ReadUShort();

            for (int i = 0; i < 14; i++)
                item.SetUnknown(i + 38, reader.ReadByte());

            return item;
        }

        #endregion

        #region IMother3Text implementation

        public string ReadItemName(int index)
        {
            int textTableAddress = rom.Settings.BankAddresses["TextTable"];
            int itemNamesTableAddress = ReadPointerFromOffsetTable(textTableAddress, 2);

            FixedTableHeader header;
            int namePointer = GetFixedTablePointer(itemNamesTableAddress, index, out header);

            reader.Position = namePointer;
            return reader.ReadSimpleString(header.EntryLength * 2);
        }

        #endregion
    }
}
