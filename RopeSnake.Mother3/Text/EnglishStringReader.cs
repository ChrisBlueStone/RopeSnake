using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.IO;

namespace RopeSnake.Mother3.Text
{
    internal class EnglishStringReader : StringReader
    {
        public EnglishStringReader(Mother3Rom rom, IBinaryReader reader) : base(rom, reader) { }

        public override string ReadDialogString()
        {
            throw new NotImplementedException();
        }

        public override string ReadString(int maxLength)
        {
            throw new NotImplementedException();
        }
    }
}
