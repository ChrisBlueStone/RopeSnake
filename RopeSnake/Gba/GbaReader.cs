using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.IO;
using RopeSnake.Graphics;

namespace RopeSnake.Gba
{
    public class GbaReader : IGbaReader
    {
        private IBinaryReader reader { get; }

        public int Position
        {
            get { return reader.Position; }
            set { reader.Position = value; }
        }

        public GbaReader(ISource source) : this(new BinaryReader(source)) { }

        public GbaReader(IBinaryReader reader)
        {
            this.reader = reader;
        }

        #region IGbaReader implementation

        public int ReadPointer()
        {
            int value = reader.ReadInt();

            if (value == 0)
                return 0;

            return value & 0x1FFFFFF;
        }

        public GbaHeader ReadHeader()
        {
            reader.Position += 0xA0;
            string title = reader.ReadString(12);
            string gameCode = reader.ReadString(4);

            return new GbaHeader
            {
                Title = title,
                GameCode = gameCode
            };
        }

        public ByteArraySource ReadCompressed()
        {
            int start = Position;

            // Check for LZ77 signature
            if (reader.ReadByte() != 0x10)
                throw new Exception($"Expected LZ77 header at position 0x{start:X}");

            // Read the block length
            int length = reader.ReadByte();
            length += (reader.ReadByte() << 8);
            length += (reader.ReadByte() << 16);
            byte[] output = new byte[length];

            int bPos = 0;
            while (bPos < length)
            {
                byte ch = reader.ReadByte();
                for (int i = 0; i < 8; i++)
                {
                    switch ((ch >> (7 - i)) & 1)
                    {
                        case 0:

                            // Direct copy
                            if (bPos >= length) break;
                            output[bPos++] = reader.ReadByte();
                            break;

                        case 1:

                            // Compression magic
                            int t = (reader.ReadByte() << 8);
                            t += reader.ReadByte();
                            int n = ((t >> 12) & 0xF) + 3;    // Number of bytes to copy

                            int o = (t & 0xFFF);

                            // Copy n bytes from bPos-o to the output
                            for (int j = 0; j < n; j++)
                            {
                                if (bPos >= length) break;
                                output[bPos] = output[bPos - o - 1];
                                bPos++;
                            }

                            break;

                        default:
                            break;
                    }
                }
            }

            return new ByteArraySource(output);
        }

        public TileSet ReadCompressedTileSet(int bitDepth)
        {
            ByteArraySource decomp = ReadCompressed();
            GbaReader reader = new GbaReader(decomp);

            return reader.ReadTileSet(decomp.Length / bitDepth / 8, bitDepth);
        }

        public TileGrid ReadCompressedTileGrid(int width, int height, int tileWidth, int tileHeight)
        {
            ByteArraySource decomp = ReadCompressed();
            GbaReader reader = new GbaReader(decomp);

            // Check for correct size
            if (decomp.Length != width * height * 2)
                throw new Exception($"Decompressed block to {decomp.Length} bytes, but expected {width * height * 2} bytes");

            return reader.ReadTileGrid(width, height, tileWidth, tileHeight);
        }

        #endregion

        #region IGraphicsReader implementation

        public Color ReadColor()
        {
            int value = reader.ReadUShort();

            int r = (value & 0x1F) * 8;
            int g = ((value >> 5) & 0x1F) * 8;
            int b = ((value >> 10) & 0x1F) * 8;

            return new Color((byte)r, (byte)g, (byte)b);
        }

        public Palette ReadPalette(int paletteCount, int colorCount)
        {
            Palette palette = new Palette(paletteCount, colorCount);

            for (int i = 0; i < paletteCount; i++)
                for (int j = 0; j < colorCount; j++)
                    palette.SetColor(i, j, ReadColor());

            return palette;
        }

        public Tile ReadTile(int bitDepth)
        {
            Tile tile = new Tile(8, 8);

            switch (bitDepth)
            {
                case 4:
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            for (int x = 0; x < 8; x += 2)
                            {
                                byte temp = reader.ReadByte();
                                tile.SetPixel(x, y, (byte)(temp & 0xF));
                                tile.SetPixel(x + 1, y, (byte)((temp >> 4) & 0xF));
                            }
                        }

                        break;
                    }

                case 8:
                    {
                        for (int y = 0; y < 8; y++)
                            for (int x = 0; x < 8; x++)
                                tile.SetPixel(x, y, reader.ReadByte());

                        break;
                    }

                default:
                    throw new InvalidOperationException("Bit depth not supported");
            }

            return tile;
        }

        public TileSet ReadTileSet(int length, int bitDepth)
        {
            TileSet tileSet = TileSet.Create(8, 8, length, () =>
             {
                 Tile tile = ReadTile(bitDepth);
                 return tile;
             });

            return tileSet;
        }

        public TileGrid ReadTileGrid(int width, int height, int tileWidth, int tileHeight)
        {
            TileGrid tileGrid = TileGrid.Create(width, height, tileWidth, tileHeight, () =>
            {
                TileProperties e = ReadTileProperties();
                return e;
            });

            return tileGrid;
        }

        public TileProperties ReadTileProperties()
        {
            int value = reader.ReadUShort();

            int tileIndex = value & 0x3FF;
            bool flipX = (value & 0x400) != 0;
            bool flipY = (value & 0x800) != 0;
            int paletteIndex = (value >> 12) & 0xF;

            return new TileProperties(tileIndex, flipX, flipY, paletteIndex);
        }

        #endregion

        #region IBinaryReader implementation

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
