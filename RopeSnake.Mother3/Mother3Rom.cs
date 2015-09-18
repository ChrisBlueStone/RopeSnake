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
        public RomSettings Settings { get; set; }

        public Mother3Rom(ISource source, RomSettings settings) : base(source)
        {
            Settings = settings;
        }

        private Mother3Version DetectVersion()
        {
            if (Header.Title != "MOTHER3" || Header.GameCode != "A3UJ")
                return Mother3Version.Invalid;

            if (Source.Length != 32 * 1024 * 1024)
                return Mother3Version.Invalid;

            // TODO: finish this
            throw new NotImplementedException();
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
