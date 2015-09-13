using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.Gba;
using RopeSnake.Graphics;

namespace RopeSnake.Mother3.RomStructures
{
    public sealed class Bg
    {
        public TileGrid TileGrid { get; set; }
        public int UnknownA { get; set; }
        public int UnknownB { get; set; }

        public static implicit operator TileGrid(Bg value) => value.TileGrid;
    }
}
