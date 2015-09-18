using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.IO;

namespace RopeSnake.Mother3.Text
{
    public class EnglishStringReader : StringReader
    {
        public EnglishStringReader(Mother3Rom rom) : base(rom) { }

        public override string ReadDialogString(IBinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public override string ReadSimpleString(IBinaryReader reader, int maxLength)
        {
            throw new NotImplementedException();
        }
    }
}
