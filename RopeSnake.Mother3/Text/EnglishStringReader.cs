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
        private Dictionary<short, string> charLookup = new Dictionary<short, string>();

        public EnglishStringReader(Mother3Rom rom) : base(rom)
        {
            charLookup = rom.Settings.CharLookup;
            if (charLookup == null)
            {
                throw new Exception("The character lookup is null");
            }
        }

        public override string ReadDialogString(IBinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public override string ReadCodedString(IBinaryReader reader)
        {
            return ReadCodedStringInternal(reader, null);
        }

        public override string ReadCodedString(IBinaryReader reader, int maxLength)
        {
            return ReadCodedStringInternal(reader, maxLength);
        }

        internal string ReadCodedStringInternal(IBinaryReader reader, int? maxLength)
        {
            // TODO: control codes
            StringBuilder sb = new StringBuilder();
            int count = 0;
            short ch;

            while ((maxLength.HasValue && count++ < maxLength) && (ch = reader.ReadShort()) >= 0)
            {
                sb.Append(charLookup[ch]);
            }

            // Advance the position past the end of the string if applicable
            while (maxLength.HasValue && count++ < maxLength)
            {
                reader.ReadUShort();
            }

            return sb.ToString();
        }
    }
}
