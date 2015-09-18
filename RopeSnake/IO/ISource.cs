using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.IO
{
    public interface ISource
    {
        int Length { get; }

        byte GetByte(int offset);
        void SetByte(int offset, byte value);
    }
}
