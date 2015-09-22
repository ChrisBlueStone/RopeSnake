using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.IO;

namespace RopeSnake.Mother3.Text
{
    public sealed class JapaneseStringReader : StringReader
    {
        public JapaneseStringReader(Mother3Rom rom) : base(rom)
        {
            charLookup = rom.Settings.CharLookup;
        }

        public override string ReadDialogString(IBinaryReader reader) => ReadCodedString(reader);

        public override string ReadCodedString(IBinaryReader reader) => ReadCodedStringInternal(reader, null);

        public override string ReadCodedString(IBinaryReader reader, int maxLength) => ReadCodedStringInternal(reader, maxLength);

        internal string ReadCodedStringInternal(IBinaryReader reader, int? maxLength)
        {
            StringBuilder sb = new StringBuilder();
            int count = 0;

            for (count = 0; (!maxLength.HasValue) ||
                (maxLength.HasValue && count < maxLength);)
            {
                ControlCode code = ProcessChar(reader, sb, ref count);

                if (code != null && code.Flags.HasFlag(ControlCodeFlags.Terminate))
                {
                    break;
                }
            }

            // Advance the position past the end of the string if applicable
            while (maxLength.HasValue && count++ < maxLength)
            {
                reader.ReadShort();
            }

            return sb.ToString();
        }
    }
}
