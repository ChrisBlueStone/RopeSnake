using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RopeSnake.IO;

namespace RopeSnake.Gba
{
    public class GbaRom
    {
        public ISource Source { get; private set; }
        public GbaHeader Header { get; private set; }

        public GbaRom(byte[] array)
        {
            Source = new ByteArraySource(array);
            InitializeGba();
        }

        public GbaRom(string filePath) : this(File.ReadAllBytes(filePath)) { }

        protected void InitializeGba()
        {
            IGbaReader reader = new GbaReader(Source);
            Header = reader.ReadHeader();
        }
    }
}
