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

        #region I/O

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

        #endregion

        #region IGraphicsReader implementation

        public Color ReadColor()
        {
            Color value = ReadColorAt(Position);
            Position += 2;
            return value;
        }

        public Color ReadColorAt(int offset)
        {
            int value = ReadUShort(offset);

            int r = (value & 0x1F) * 8;
            int g = ((value >> 5) & 0x1F) * 8;
            int b = ((value >> 10) & 0x1F) * 8;

            return new Color((byte)r, (byte)g, (byte)b);
        }

        public Palette ReadPalette() => ReadPalette(1);

        public Palette ReadPalette(int paletteCount) => ReadPalette(paletteCount, 16);

        public Palette ReadPalette(int paletteCount, int colorCount)
        {
            Palette value = ReadPaletteAt(Position, paletteCount, colorCount);
            Position += paletteCount * colorCount * 2;
            return value;
        }

        public Palette ReadPaletteAt(int offset) => ReadPaletteAt(offset, 1);

        public Palette ReadPaletteAt(int offset, int paletteCount) => ReadPaletteAt(offset, paletteCount, 16);

        public Palette ReadPaletteAt(int offset, int paletteCount, int colorCount)
        {
            Palette palette = new Palette(paletteCount, colorCount);

            for (int i = 0; i < paletteCount; i++)
            {
                for (int j = 0; j < colorCount; j++)
                {
                    palette.SetColor(i, j, ReadColorAt(offset));
                    offset += 2;
                }
            }

            return palette;
        }

        public Tile ReadTile() => ReadTile(4);

        public Tile ReadTile(int bitDepth)
        {
            Tile value = ReadTileAt(Position, bitDepth);
            Position += bitDepth * 8;
            return value;
        }

        public Tile ReadTileAt(int offset) => ReadTileAt(offset, 4);

        public Tile ReadTileAt(int offset, int bitDepth)
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

        public TileSet ReadTileSet(int length) => ReadTileSet(length, 4);

        public TileSet ReadTileSet(int length, int bitDepth)
        {
            TileSet value = ReadTileSetAt(Position, length, bitDepth);
            Position += length * bitDepth * 8;
            return value;
        }

        public TileSet ReadTileSetAt(int offset, int length) => ReadTileSetAt(offset, length, 4);

        public TileSet ReadTileSetAt(int offset, int length, int bitDepth)
        {
            TileSet tileSet = TileSet.Create(8, 8, length, () =>
             {
                 Tile tile = ReadTileAt(offset, bitDepth);
                 offset += bitDepth * 8;
                 return tile;
             });

            return tileSet;
        }

        public TileGrid ReadTileGrid(int width, int height) => ReadTileGrid(width, height, 8, 8);

        public TileGrid ReadTileGridAt(int offset, int width, int height) => ReadTileGridAt(offset, width, height, 8, 8);

        public TileGrid ReadTileGrid(int width, int height, int tileWidth, int tileHeight)
        {
            TileGrid value = ReadTileGridAt(Position, width, height, tileWidth, tileHeight);
            Position += width * height * 2;
            return value;
        }

        public TileGrid ReadTileGridAt(int offset, int width, int height, int tileWidth, int tileHeight)
        {
            TileGrid tileGrid = TileGrid.Create(width, height, tileWidth, tileHeight, () =>
            {
                TileProperties e = ReadTilePropertiesAt(offset);
                offset += 2;
                return e;
            });

            return tileGrid;
        }

        public TileProperties ReadTileProperties()
        {
            TileProperties value = ReadTilePropertiesAt(Position);
            Position += 2;
            return value;
        }

        public TileProperties ReadTilePropertiesAt(int offset)
        {
            int value = ReadUShort(offset);

            int tileIndex = value & 0x3FF;
            bool flipX = (value & 0x400) != 0;
            bool flipY = (value & 0x800) != 0;
            int paletteIndex = (value >> 12) & 0xF;

            return new TileProperties(tileIndex, flipX, flipY, paletteIndex);
        }

        public TileSet ReadCompressedTileSet(int offset, int bitDepth)
        {
            Source decomp = ReadCompressed(offset);
            GbaReader reader = new GbaReader(decomp);
            return reader.ReadTileSetAt(0, decomp.Length / bitDepth / 8, bitDepth);
        }

        #endregion
    }
}
