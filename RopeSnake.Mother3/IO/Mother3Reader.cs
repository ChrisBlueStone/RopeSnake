using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.Gba;
using RopeSnake.Graphics;
using RopeSnake.IO;
using RopeSnake.Mother3.Data;
using RopeSnake.Mother3.Text;

namespace RopeSnake.Mother3.IO
{
    public class Mother3Reader : IMother3Reader
    {
        private IGbaReader reader;
        private Mother3Rom rom;

        // Strings are complicated, so the implementation of ReadDialogString and ReadCodedString are deferred
        // to a separate class called StringReader, of which there may be different implementations depending
        // on which language the ROM is in
        private StringReader stringReader;

        public int Position
        {
            get { return reader.Position; }
            set { reader.Position = value; }
        }

        public Mother3Reader(Mother3Rom rom)
        {
            reader = new GbaReader(rom.Source);
            this.rom = rom;

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
        }

        #region IMother3Reader implementation

        public FixedTableHeader ReadFixedTableHeader()
        {
            int entryLength = reader.ReadUShort();
            int count = reader.ReadUShort();
            return new FixedTableHeader(entryLength, count);
        }

        public Bg ReadBg()
        {
            // Check for alignment
            if (!reader.Position.IsAligned(4))
                throw new AlignmentException(reader.Position, 4);

            // Check for "bg  " header
            string header = reader.ReadString(4);

            if (header != "bg  ")
                throw new Exception($"Unexpected header. Expected \"bg  \", actual \"{header}\"");

            // Read data
            int unknownA = reader.ReadInt();
            int unknownB = reader.ReadInt();
            TileGrid grid = reader.ReadCompressedTileGrid(32, 32, 8, 8);

            // Check for "~bg " footer
            reader.Position = reader.Position.Align(4);
            string footer = reader.ReadString(4);

            if (footer != "~bg ")
                throw new Exception($"Unexpected footer. Expected \"~bg \", actual \"{header}\"");

            return new Bg
            {
                UnknownA = unknownA,
                UnknownB = unknownB,
                TileGrid = grid
            };
        }

        public Item ReadItem()
        {
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

        public string ReadDialogString() => stringReader.ReadDialogString(reader);

        public string ReadCodedString(int maxLength) => stringReader.ReadCodedString(reader, maxLength);

        public int[] ReadOffsetTable()
        {
            int basePosition = Position;
            int count = ReadInt();
            int[] offsets = new int[count + 1];

            // There's always an extra offset at the end denoting the address just
            // after the table
            for (int i = 0; i <= count; i++)
            {
                int offset = ReadInt();
                offsets[i] = basePosition + offset;
            }

            return offsets;
        }

        #endregion

        #region IGbaReader implementation

        public ByteArraySource ReadCompressed() => reader.ReadCompressed();

        public TileSet ReadCompressedTileSet(int bitDepth) => reader.ReadCompressedTileSet(bitDepth);

        public TileGrid ReadCompressedTileGrid(int width, int height, int tileWidth, int tileHeight)
            => reader.ReadCompressedTileGrid(width, height, tileWidth, tileHeight);

        public GbaHeader ReadHeader() => reader.ReadHeader();

        public int ReadPointer() => reader.ReadPointer();

        public Color ReadColor() => reader.ReadColor();

        public Palette ReadPalette(int paletteCount, int colorCount) => reader.ReadPalette(paletteCount, colorCount);

        public Tile ReadTile(int bitDepth) => reader.ReadTile(bitDepth);

        public TileSet ReadTileSet(int length, int bitDepth) => reader.ReadTileSet(length, bitDepth);

        public TileProperties ReadTileProperties() => reader.ReadTileProperties();

        public TileGrid ReadTileGrid(int width, int height, int tileWidth, int tileHeight)
            => reader.ReadTileGrid(width, height, tileWidth, tileHeight);

        public byte[] ReadByteArray(int size) => reader.ReadByteArray(size);

        public ByteArraySource ReadByteArraySource(int size) => reader.ReadByteArraySource(size);

        public int ReadInt() => reader.ReadInt();

        public byte ReadByte() => reader.ReadByte();

        public sbyte ReadSByte() => reader.ReadSByte();

        public short ReadShort() => reader.ReadShort();

        public string ReadString() => reader.ReadString();

        public string ReadString(int maxLength) => reader.ReadString(maxLength);

        public uint ReadUInt() => reader.ReadUInt();

        public ushort ReadUShort() => reader.ReadUShort();

        #endregion
    }
}
