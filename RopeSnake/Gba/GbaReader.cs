using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.IO;
using RopeSnake.Graphics;

namespace RopeSnake.Gba
{
    public class GbaReader : BinaryReader, IGraphicsReader
    {
        public GbaReader(Source source) : base(source) { }

        public Color ReadColor()
        {
            Color value = ReadColor(Position);
            Position += 2;
            return value;
        }

        public Color ReadColor(int offset)
        {
            int value = ReadUShort(offset);

            int r = (value & 0x1F) * 8;
            int g = ((value >> 5) & 0x1F) * 8;
            int b = ((value >> 10) & 0x1F) * 8;

            return new Color((byte)r, (byte)g, (byte)b);
        }

        public Palette ReadPalette() => ReadPalette(1, 16);

        public Palette ReadPalette(int paletteCount, int colorCount)
        {
            Palette value = ReadPalette(Position, paletteCount, colorCount);
            Position += paletteCount * colorCount * 2;
            return value;
        }

        public Palette ReadPalette(int offset, int paletteCount, int colorCount)
        {
            Palette palette = new Palette(paletteCount, colorCount);

            for (int i = 0; i < paletteCount; i++)
            {
                for (int j = 0; j < colorCount; j++)
                {
                    palette.SetColor(i, j, ReadColor(offset));
                    offset += 2;
                }
            }

            return palette;
        }

        public Tile ReadTile()
        {
            Tile value = ReadTile(Position, 4);
            Position += 32;
            return value;
        }

        public Tile ReadTile(int offset) => ReadTile(offset, 4);

        public Tile ReadTile(int offset, int bitDepth)
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
                                byte temp = ReadByte(offset++);
                                tile.SetPixel(x, y, (byte)(temp & 0xF));
                                tile.SetPixel(x + 1, y, (byte)((temp >> 4) & 0xF));
                            }
                        }
                        break;
                    }

                case 8:
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            for (int x = 0; x < 8; x++)
                            {
                                tile.SetPixel(x, y, ReadByte(offset++));
                            }
                        }
                        break;
                    }

                default:
                    throw new InvalidOperationException("Bit depth not supported");
            }

            return tile;
        }

        public TileSet ReadTileSet(int length)
        {
            TileSet value = ReadTileSet(Position, length, 4);
            Position += length * 32;
            return value;
        }

        public TileSet ReadTileSet(int offset, int length) => ReadTileSet(offset, length, 4);

        public TileSet ReadTileSet(int offset, int length, int bitDepth)
        {
            TileSet tileSet = TileSet.Create(8, 8, length, () =>
             {
                 Tile tile = ReadTile(offset, bitDepth);
                 offset += bitDepth * 8;
                 return tile;
             });

            return tileSet;
        }

        public TileGrid ReadTileGrid(int width, int height) => ReadTileGrid(width, height, 8, 8);

        public TileGrid ReadTileGrid(int offset, int width, int height) => ReadTileGrid(offset, width, height, 8, 8);

        public TileGrid ReadTileGrid(int width, int height, int tileWidth, int tileHeight)
        {
            TileGrid value = ReadTileGrid(Position, width, height, tileWidth, tileHeight);
            Position += width * height * 2;
            return value;
        }

        public TileGrid ReadTileGrid(int offset, int width, int height, int tileWidth, int tileHeight)
        {
            TileGrid tileGrid = TileGrid.Create(width, height, tileWidth, tileHeight, () =>
            {
                TileProperties e = ReadTileProperties(offset);
                offset += 2;
                return e;
            });

            return tileGrid;
        }

        public TileProperties ReadTileProperties()
        {
            TileProperties value = ReadTileProperties(Position);
            Position += 2;
            return value;
        }

        public TileProperties ReadTileProperties(int offset)
        {
            int value = ReadUShort(offset);

            int tileIndex = value & 0x3FF;
            bool flipX = (value & 0x400) != 0;
            bool flipY = (value & 0x800) != 0;
            int paletteIndex = (value >> 12) & 0xF;

            return new TileProperties(tileIndex, flipX, flipY, paletteIndex);
        }

        public GbaHeader ReadHeader(int offset)
        {
            string title = ReadString(offset + 0xA0, 12);
            string gameCode = ReadString(offset + 0xAC, 4);

            return new GbaHeader
            {
                Title = title,
                GameCode = gameCode
            };
        }

        public ByteArraySource ReadCompressed(int offset)
        {
            byte[] array = Lz77.DecompLZ77(Source, offset);
            return new ByteArraySource(array);
        }

        public TileSet ReadCompressedTileSet(int offset, int bitDepth)
        {
            Source decomp = ReadCompressed(offset);
            GbaReader reader = new GbaReader(decomp);
            return reader.ReadTileSet(0, decomp.Length / bitDepth / 8, bitDepth);
        }
    }
}
