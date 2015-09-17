using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.Mother3.IO
{
    public struct SizeLoc
    {
        public readonly int Location;
        public readonly int Size;

        public SizeLoc(int location, int size)
        {
            Location = location;
            Size = size;
        }
    }
}
