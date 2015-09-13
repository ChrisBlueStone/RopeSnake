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
            Mother3Rom rom = new Mother3Rom(new ByteArraySource("mother3_jp.gba"), null);
            GbaReader reader = new GbaReader(rom.Source);

            reader.Position = 0x1BD4338;
            TileSet tileSet = reader.ReadCompressedTileSet(8);

            reader.Position = 0x1BD5F40;
            Palette palette = reader.ReadPalette(1, 256);

            reader.Position = 0x1BD6140;
            TileGrid tileGrid = reader.ReadTileGrid(32, 32, 8, 8);

            Bitmap titleScreen = Renderer.RenderGrid(tileSet, palette, tileGrid, false);
            titleScreen.Save("titlescreen.png");
        }
    }
}
