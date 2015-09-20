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

        protected StringReader(Mother3Rom rom)
        {
            this.rom = rom;
        }

        public abstract string ReadDialogString(IBinaryReader reader);

        public abstract string ReadString(IBinaryReader reader, int maxLength);
    }
}
