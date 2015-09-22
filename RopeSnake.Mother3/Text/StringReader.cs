using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.IO;

namespace RopeSnake.Mother3.Text
{
    public abstract class StringReader
    {
        protected Dictionary<short, string> charLookup;
        protected IEnumerable<ControlCode> controlCodes;

        protected StringReader(Mother3Rom rom)
        {
            controlCodes = rom.Settings.ControlCodes;
            if (controlCodes == null)
            {
                throw new Exception("The control codes collection is null");
            }
        }

        public abstract string ReadDialogString(IBinaryReader reader);

        public abstract string ReadCodedString(IBinaryReader reader);

        public abstract string ReadCodedString(IBinaryReader reader, int maxLength);

        internal virtual ControlCode ProcessChar(IBinaryReader reader, StringBuilder sb, ref int count)
        {
            short ch = reader.ReadShort();
            count++;

            ControlCode code = controlCodes.FirstOrDefault(cc => cc.Code == ch);

            if (code != null)
            {
                if (code.Code != -1)
                {
                    sb.Append('[');

                    if (code.Tag != null)
                    {
                        sb.Append(code.Tag);
                    }
                    else
                    {
                        sb.Append(((ushort)ch).ToString("X4"));
                    }

                    for (int i = 0; i < code.Arguments; i++)
                    {
                        ch = reader.ReadShort();
                        count++;

                        sb.Append(' ');
                        sb.Append(((ushort)ch).ToString("X4"));
                    }

                    sb.Append(']');
                }
            }
            else
            {
                if (charLookup.ContainsKey(ch))
                {
                    sb.Append(charLookup[ch]);
                }
                else
                {
                    sb.Append('[');
                    sb.Append(((ushort)ch).ToString("X4"));
                    sb.Append(']');
                }
            }

            return code;
        }
    }
}
