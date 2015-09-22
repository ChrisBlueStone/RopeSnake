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
        private bool encoded;
        private ScriptEncodingParameters encodingParameters;
        private byte[] graphicsPad;
        private byte[] codePad;

        public EnglishStringReader(Mother3Rom rom) : base(rom)
        {
            charLookup = rom.Settings.CharLookup;
            if (charLookup == null)
            {
                throw new Exception("The character lookup is null");
            }

            if ((rom.Settings.Version == Mother3Version.English10) || (rom.Settings.Version == Mother3Version.English11))
            {
                encoded = true;

                // Get the encoding parameters
                BinaryReader reader = new BinaryReader(rom.Source);
                encodingParameters = rom.Settings.ScriptEncoding;

                if (encodingParameters == null)
                {
                    throw new Exception("Script encoding parameters cannot be null");
                }

                reader.Position = encodingParameters.EvenPadAddress;
                graphicsPad = reader.ReadByteArray(encodingParameters.EvenPadModulus);

                reader.Position = encodingParameters.OddPadAddress;
                codePad = reader.ReadByteArray(encodingParameters.OddPadModulus);
            }
            else
            {
                encoded = false;
            }
        }

        public override string ReadDialogString(IBinaryReader reader)
        {
            List<byte> decoded = new List<byte>();
            bool first = true;

            for (;;)
            {
                byte value = DecodeByte(reader);

                if (value == 0xEF)
                {
                    // Custom article code
                    decoded.Add(0xEF);
                    decoded.Add(DecodeByte(reader));
                }

                else if (first && value == 0xFE)
                {
                    // Custom hotspot code
                    decoded.Add(0xFF);
                    decoded.Add(0xFE);
                    decoded.Add(DecodeByte(reader));
                    decoded.Add(DecodeByte(reader));
                }

                // Check for other control codes
                else if (value >= 0xF0)
                {
                    if (value == 0xFF)
                    {
                        decoded.Add(0xFF);
                        decoded.Add(0xFF);
                        break;
                    }

                    else
                    {
                        int length = (value & 0xF) * 2;
                        byte code = DecodeByte(reader);

                        decoded.Add(code);
                        decoded.Add(0xFF);

                        for (int i = 0; i < length; i++)
                        {
                            decoded.Add(DecodeByte(reader));
                        }
                    }
                }

                else
                {
                    decoded.Add(value);
                    decoded.Add(0);
                }

                first = false;
            }

            BinaryReader decodedReader = new BinaryReader(new ByteArraySource(decoded.ToArray()));

            return ReadCodedString(decodedReader);
        }

        internal byte DecodeByte(IBinaryReader reader)
        {
            int pos = reader.Position + 0x8000000;
            byte raw = reader.ReadByte();

            if (!encoded)
            {
                return raw;
            }

            bool even = (pos & 1) == 0;

            if (even)
            {
                int offset = ((pos >> 1) % encodingParameters.EvenPadModulus);
                byte pad = graphicsPad[offset];
                return (byte)(((raw + encodingParameters.EvenOffset1) ^ pad) + encodingParameters.EvenOffset2);
            }
            else
            {
                int offset = ((pos >> 1) % encodingParameters.OddPadModulus);
                byte pad = codePad[offset];
                return (byte)(((raw + encodingParameters.OddOffset1) ^ pad) + encodingParameters.OddOffset2);
            }
        }

        public override string ReadCodedString(IBinaryReader reader) => ReadCodedStringInternal(reader, null);

        public override string ReadCodedString(IBinaryReader reader, int maxLength) => ReadCodedStringInternal(reader, maxLength);

        internal string ReadCodedStringInternal(IBinaryReader reader, int? maxLength)
        {
            StringBuilder sb = new StringBuilder();
            int count = 0;

            for (count = 0; (!maxLength.HasValue) ||
                (maxLength.HasValue && count < maxLength);)
            {
                // Check for custom codes
                ushort peek = reader.PeekUShort();
                byte upper = (byte)((peek >> 8) & 0xFF);

                if (upper == 0xEF)
                {
                    // Custom code with one-byte argument inline
                    reader.ReadByte();
                    byte arg = reader.ReadByte();
                    count++;

                    sb.Append($"[EF{arg:X2}]");
                }

                else if (peek == 0xFEFF)
                {
                    // Custom code (hot springs) with two-byte argument following
                    reader.ReadShort();
                    ushort arg = reader.ReadUShort();
                    count += 2;

                    sb.Append($"[HOTSPRING {arg:X4}]");
                }

                else
                {
                    ControlCode code = ProcessChar(reader, sb, ref count);

                    if (code != null && code.Flags.HasFlag(ControlCodeFlags.Terminate))
                    {
                        break;
                    }
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
