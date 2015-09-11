using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.IO;

namespace RopeSnake.Graphics
{
    public interface IGraphicsReader : IBinaryReader
    {
        Color ReadColor();
        Palette ReadPalette(int paletteCount, int colorCount);
        Tile ReadTile(int bitDepth);
        TileSet ReadTileSet(int length, int bitDepth);
        TileProperties ReadTileProperties();
        TileGrid ReadTileGrid(int width, int height, int tileWidth, int tileHeight);
    }
}
