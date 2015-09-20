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
        private static Dictionary<byte, string> charLookup = new Dictionary<byte, string>();

        static EnglishStringReader()
        {
            // TODO: charLookup
        }

        public EnglishStringReader(Mother3Rom rom) : base(rom) { }

        public override string ReadDialogString(IBinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public override string ReadString(IBinaryReader reader, int maxLength)
        {
            // TODO: control codes
            StringBuilder sb = new StringBuilder();
            int count = 0;
            short ch;

            while ((ch = reader.ReadShort()) >= 0 && count++ < maxLength)
            {
                sb.Append(charLookup[(byte)ch]);
            }

            return sb.ToString();
        }
    }
}
