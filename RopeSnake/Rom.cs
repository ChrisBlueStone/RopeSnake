using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.IO;

namespace RopeSnake
{
    public class Rom
    {
        public ISource Source { get; protected set; }

        public Rom(ISource source)
        {
            Source = source;
        }
    }
}
