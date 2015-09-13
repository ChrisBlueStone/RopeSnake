using RopeSnake.Graphics;
using RopeSnake.IO;

namespace RopeSnake.Gba
{
    public interface IGbaReader : IBinaryReader, IGraphicsReader
    {
        ByteArraySource ReadCompressed();
        TileSet ReadCompressedTileSet(int bitDepth);
        TileGrid ReadCompressedTileGrid(int width, int height, int tileWidth, int tileHeight);
        GbaHeader ReadHeader();
        int ReadPointer();
    }
}