using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.Graphics
{
    public interface IGraphicsReader
    {
        Color ReadColor();
        Color ReadColorAt(int offset);

        Palette ReadPalette();
        Palette ReadPalette(int paletteCount);
        Palette ReadPalette(int paletteCount, int colorCount);
        Palette ReadPaletteAt(int offset);
        Palette ReadPaletteAt(int offset, int paletteCount);
        Palette ReadPaletteAt(int offset, int paletteCount, int colorCount);

        Tile ReadTile();
        Tile ReadTile(int bitDepth);
        Tile ReadTileAt(int offset);
        Tile ReadTileAt(int offset, int bitDepth);

        TileSet ReadTileSet(int length);
        TileSet ReadTileSet(int length, int bitDepth);
        TileSet ReadTileSetAt(int offset, int length);
        TileSet ReadTileSetAt(int offset, int length, int bitDepth);

        TileProperties ReadTileProperties();
        TileProperties ReadTilePropertiesAt(int offset);

        TileGrid ReadTileGrid(int width, int height);
        TileGrid ReadTileGrid(int width, int height, int tileWidth, int tileHeight);
        TileGrid ReadTileGridAt(int offset, int width, int height);
        TileGrid ReadTileGridAt(int offset, int width, int height, int tileWidth, int tileHeight);
    }
}
