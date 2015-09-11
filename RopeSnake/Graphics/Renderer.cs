using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace RopeSnake.Graphics
{
    public static unsafe class Renderer
    {
        public static Bitmap RenderGrid(TileSet tileSet, Palette palette, TileGrid tileGrid, bool transparent)
        {
            Bitmap bitmap = new Bitmap(tileGrid.Width * tileGrid.TileWidth, tileGrid.Height * tileGrid.TileHeight,
                PixelFormat.Format32bppArgb);

            BitmapData canvas = bitmap.LockBits();
            RenderGrid(canvas, 0, 0, tileSet, palette, tileGrid, transparent);
            bitmap.UnlockBits(canvas);

            return bitmap;
        }

        public static void RenderGrid(BitmapData canvas, int x, int y, TileSet tileSet, Palette palette, TileGrid tileGrid,
            bool transparent)
        {
            for (int ty = 0; ty < tileGrid.Height; ty++)
            {
                for (int tx = 0; tx < tileGrid.Width; tx++)
                {
                    TileProperties properties = tileGrid[tx, ty];

                    RenderTile(canvas, x + (tx * tileGrid.TileWidth), y + (ty * tileGrid.TileHeight), tileSet[properties.TileIndex],
                        palette, properties.PaletteIndex, properties.FlipX, properties.FlipY, transparent);
                }
            }
        }

        public static void RenderTile(BitmapData canvas, int x, int y, Tile tile, Palette palette, int paletteIndex,
            bool flipX, bool flipY, bool transparent)
        {
            int* start = (int*)canvas.Scan0 + canvas.Stride * y / 4 + x;

            for (int py = 0; py < 8; py++)
            {
                int* current = start;
                int actualY = flipY ? (7 - py) : py;

                for (int px = 0; px < 8; px++)
                {
                    byte colorIndex = tile[flipX ? (7 - px) : px, actualY];

                    if (!transparent || (colorIndex != 0))
                    {
                        *current = palette.GetColor(paletteIndex, colorIndex).Argb;
                    }

                    current++;
                }

                start += canvas.Stride / 4;
            }
        }
    }
}
