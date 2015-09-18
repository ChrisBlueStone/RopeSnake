using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.Mother3.IO
{
    public struct FixedTableHeader
    {
        public readonly int EntryLength;
        public readonly int Count;

        public FixedTableHeader(int entryLength, int count)
        {
            EntryLength = entryLength;
            Count = count;
        }
    }
}
