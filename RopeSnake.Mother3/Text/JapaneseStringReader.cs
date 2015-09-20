using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.IO;

namespace RopeSnake.Mother3.Text
{
    internal sealed class JapaneseStringReader : StringReader
    {
        private Dictionary<short, string> charLookup = new Dictionary<short, string>();

        private static Encoding sjisEncoding = Encoding.GetEncoding(932);

        public JapaneseStringReader(Mother3Rom rom) : base(rom)
        {
            // Build a lookup table from the font metadata
            IBinaryReader sjisReader = new BinaryReader(rom.Source, true);
            sjisReader.Position = rom.Settings.BankAddresses["MainFont"];

            for (int i = 0; i < 7332; i++)
            {
                byte[] sjis = sjisReader.ReadByteArray(2);
                sjisReader.Position += 20;

                string value = sjisEncoding.GetString(sjis);
                charLookup.Add((short)i, value);
            }
        }

        public override string ReadDialogString(IBinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public override string ReadCodedString(IBinaryReader reader, int maxLength)
        {
            // TODO: control codes
            StringBuilder sb = new StringBuilder();
            int count = 0;
            short ch;

            while ((ch = reader.ReadShort()) >= 0 && count++ < maxLength)
            {
                sb.Append(charLookup[ch]);
            }

            return sb.ToString();
        }
    }
}
