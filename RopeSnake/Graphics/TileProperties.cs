using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.Graphics
{
    public class TileProperties
    {
        public int TileIndex { get; set; }
        public bool FlipX { get; set; }
        public bool FlipY { get; set; }
        public int PaletteIndex { get; set; }

        public TileProperties() { }

        public TileProperties(int tileIndex, bool flipX, bool flipY, int paletteIndex)
        {
            TileIndex = tileIndex;
            FlipX = flipX;
            FlipY = flipY;
            PaletteIndex = paletteIndex;
        }

        public TileProperties(TileProperties copyFrom, bool flipX, bool flipY)
        {
            TileIndex = copyFrom.TileIndex;
            PaletteIndex = copyFrom.PaletteIndex;
            FlipX = flipX;
            FlipY = flipY;
        }
    }
}
