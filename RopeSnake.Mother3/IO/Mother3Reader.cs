using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.Gba;
using RopeSnake.Graphics;
using RopeSnake.IO;
using RopeSnake.Mother3.RomStructures;

namespace RopeSnake.Mother3.IO
{
    public class Mother3Reader : IMother3Reader
    {
        private IGbaReader reader;

        public int Position
        {
            get { return reader.Position; }
            set { reader.Position = value; }
        }

        public Mother3Reader(IGbaReader reader)
        {
            this.reader = reader;
        }

        #region IMother3Reader implementation

        public Bg ReadBg()
        {
            // Check for alignment
            if (!Position.IsAligned(4))
                throw new AlignmentException(Position, 4);

            // Check for "bg  " header
            string header = reader.ReadString(4);

            if (header != "bg  ")
                throw new Exception($"Unexpected header. Expected \"bg  \", actual \"{header}\"");

            // Read data
            int unknown1 = reader.ReadInt();
            int unknown2 = reader.ReadInt();
            TileGrid grid = reader.ReadCompressedTileGrid(32, 32, 8, 8);

            // Check for "~bg " footer
            Position = Position.Align(4);
            string footer = reader.ReadString(4);

            if (footer != "~bg ")
                throw new Exception($"Unexpected footer. Expected \"~bg \", actual \"{header}\"");

            return new Bg
            {
                Unknown1 = unknown1,
                Unknown2 = unknown2,
                TileGrid = grid
            };
        }

        #endregion

        #region IGraphicsReader implementation

        public Color ReadColor() => reader.ReadColor();

        public Tile ReadTile(int bitDepth) => reader.ReadTile(bitDepth);

        public TileGrid ReadTileGrid(int width, int height, int tileWidth, int tileHeight)
            => reader.ReadTileGrid(width, height, tileWidth, tileHeight);

        public TileProperties ReadTileProperties() => reader.ReadTileProperties();

        public TileSet ReadTileSet(int length, int bitDepth) => reader.ReadTileSet(length, bitDepth);

        #endregion

        #region IGbaReader implementation

        public ByteArraySource ReadCompressed() => reader.ReadCompressed();

        public TileSet ReadCompressedTileSet(int bitDepth) => reader.ReadCompressedTileSet(bitDepth);

        public TileGrid ReadCompressedTileGrid(int width, int height, int tileWidth, int tileHeight)
            => reader.ReadCompressedTileGrid(width, height, tileWidth, tileHeight);

        public GbaHeader ReadHeader() => reader.ReadHeader();

        public int ReadPointer() => reader.ReadPointer();

        #endregion

        #region IBinaryReader implementation

        public byte ReadByte() => reader.ReadByte();

        public byte[] ReadByteArray(int size) => reader.ReadByteArray(size);

        public ByteArraySource ReadByteArraySource(int size) => reader.ReadByteArraySource(size);

        public int ReadInt() => reader.ReadInt();

        public Palette ReadPalette(int paletteCount, int colorCount) => reader.ReadPalette(paletteCount, colorCount);

        public sbyte ReadSByte() => reader.ReadSByte();

        public short ReadShort() => reader.ReadShort();

        public string ReadString() => reader.ReadString();

        public string ReadString(int maxLength) => reader.ReadString(maxLength);

        public uint ReadUInt() => reader.ReadUInt();

        public ushort ReadUShort() => reader.ReadUShort();

        #endregion
    }
}
