using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.IO
{
    public static class IOHelpers
    {
        public static int Align(this int pointer, int alignment)
        {
            if (alignment < 1)
                throw new ArgumentException("Alignment must be positive");

            if (alignment == 1)
                return pointer;

            int mask = ~alignment;
            pointer += alignment - 1;
            pointer &= mask;
            return pointer;
        }

        public static bool IsAligned(this int pointer, int alignment)
        {
            if (alignment < 1)
                throw new ArgumentException("Alignment must be positive");

            if (alignment == 1)
                return true;

            int mask = ~alignment;
            return pointer == (pointer & mask);
        }
    }
}
