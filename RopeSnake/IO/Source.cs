using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.IO
{
    public abstract class Source
    {
        public abstract int Length { get; }

        public abstract byte ReadByte(int offset);
        public abstract void WriteByte(int offset, byte value);
    }
}
