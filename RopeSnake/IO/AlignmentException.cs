using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.IO
{
    public sealed class AlignmentException : Exception
    {
        public int Offset { get; set; }
        public int ExpectedAlignment { get; set; }

        public AlignmentException(int offset, int alignment)
            : base($"Invalid alignment of offset 0x{offset:X}: must be aligned by {alignment}")
        { }
    }
}
