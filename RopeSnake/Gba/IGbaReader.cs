using RopeSnake.Graphics;
using RopeSnake.IO;

namespace RopeSnake.Gba
{
    public interface IGbaReader : IBinaryReader
    {
        ByteArraySource ReadCompressed();
        TileSet ReadCompressedTileSet(int bitDepth);
        GbaHeader ReadHeader();
        int ReadPointer();
    }
}