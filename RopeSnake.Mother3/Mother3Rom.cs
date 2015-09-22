using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.Gba;
using RopeSnake.IO;
using RopeSnake.Mother3.IO;
using RopeSnake.Mother3.Data;

namespace RopeSnake.Mother3
{
    public sealed class Mother3Rom : GbaRom
    {
        private static Encoding sjisEncoding = Encoding.GetEncoding(932);

        public RomSettings Settings { get; set; }

        public Mother3Rom(ISource source, RomSettings settings) : base(source)
        {
            Settings = settings;

            if (settings.CharLookup == null)
            {
                ReadCharLookup();
            }

            if (settings.ScriptEncoding != null)
            {
                ReadEncodingPadData();
            }
        }

        private void ReadCharLookup()
        {
            Settings.CharLookup = new Dictionary<short, string>();

            // Build a lookup table from the font metadata
            IBinaryReader sjisReader = new BinaryReader(Source, true);
            sjisReader.Position = Settings.BankAddresses["MainFont"];

            for (int i = 0; i < 7332; i++)
            {
                byte[] sjis = sjisReader.ReadByteArray(2);
                sjisReader.Position += 20;

                string value = sjisEncoding.GetString(sjis);
                Settings.CharLookup.Add((short)i, value);
            }
        }

        private void ReadEncodingPadData()
        {
            BinaryReader reader = new BinaryReader(Source);
            ScriptEncodingParameters encodingParameters = Settings.ScriptEncoding;

            reader.Position = encodingParameters.EvenPadAddress;
            encodingParameters.EvenPad = reader.ReadByteArray(encodingParameters.EvenPadModulus);

            reader.Position = encodingParameters.OddPadAddress;
            encodingParameters.OddPad = reader.ReadByteArray(encodingParameters.OddPadModulus);
        }
    }

    public enum Mother3Version
    {
        Invalid,
        None,
        Japanese,
        English10,
        English11,
        English12
    }
}
