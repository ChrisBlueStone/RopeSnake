using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.IO;

namespace RopeSnake.Mother3.Text
{
    internal abstract class StringReader
    {
        protected Mother3Rom rom;
        protected IBinaryReader reader;

        protected StringReader(Mother3Rom rom, IBinaryReader reader)
        {
            this.rom = rom;
            this.reader = reader;
        }

        public abstract string ReadDialogString();

        public abstract string ReadString(int maxLength);
    }
}
