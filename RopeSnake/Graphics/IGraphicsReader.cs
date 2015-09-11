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
        Color ReadColor(int offset);

        Palette ReadPalette();
        Palette ReadPalette(int paletteCount, int colorCount);
        Palette ReadPalette(int offset, int paletteCount, int colorCount);

        Tile ReadTile();
        Tile ReadTile(int offset);
        Tile ReadTile(int offset, int bitDepth);

        TileSet ReadTileSet(int length);
        TileSet ReadTileSet(int offset, int length);
        TileSet ReadTileSet(int offset, int length, int bitDepth);

        TileProperties ReadTileProperties();
        TileProperties ReadTileProperties(int offset);

        TileGrid ReadTileGrid(int width, int height);
        TileGrid ReadTileGrid(int offset, int width, int height);
        TileGrid ReadTileGrid(int width, int height, int tileWidth, int tileHeight);
        TileGrid ReadTileGrid(int offset, int width, int height, int tileWidth, int tileHeight);
    }
}
