using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using RopeSnake.Mother3;
using RopeSnake.Graphics;
using RopeSnake.Gba;
using RopeSnake.IO;

namespace RopeSnake.Mother3.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Mother3Rom rom = new Mother3Rom("mother3.gba");
            GbaReader reader = new GbaReader(rom);

            TileSet tileSet = reader.ReadCompressedTileSet(0x1BD4338, 8);
            Palette palette = reader.ReadPaletteAt(0x1BD5F40, 1, 256);
            TileGrid tileGrid = reader.ReadTileGridAt(0x1BD6140, 32, 32);

            Bitmap titleScreen = Renderer.RenderGrid(tileSet, palette, tileGrid, false);
            titleScreen.Save("titlescreen.png");
        }
    }
}
