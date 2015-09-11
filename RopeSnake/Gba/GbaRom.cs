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
        public ByteArraySource Source { get; private set; }
        public GbaHeader Header { get; private set; }

        public GbaRom(byte[] array)
        {
            Source = new ByteArraySource(array);
            Initialize();
        }

        public GbaRom(string filePath) : this(File.ReadAllBytes(filePath)) { }

        protected void Initialize()
        {
            GbaReader reader = new GbaReader(Source);
            Header = reader.ReadHeader(0);
        }

        public static implicit operator Source(GbaRom gbaRom)
        {
            return gbaRom.Source;
        }
    }
}
