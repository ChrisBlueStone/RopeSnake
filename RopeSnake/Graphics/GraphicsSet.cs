using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.Graphics
{
    public class GraphicsSet
    {
        public TileSet TileSet { get; set; }
        public Palette Palette { get; set; }
        public TileGrid tileGrid { get; set; }
    }
}
