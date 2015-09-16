using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RopeSnake.IO;

namespace RopeSnake.Gba
{
    public class GbaRom : Rom
    {
        public GbaHeader Header { get; private set; }

        public GbaRom(ISource source) : base(source)
        {
            InitializeGba();
        }

        protected void InitializeGba()
        {
            IGbaReader reader = new GbaReader(Source);
            Header = reader.ReadHeader();
        }
    }
}
