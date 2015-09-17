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
        public Mother3Version Version { get; set; }

        public Mother3RomSettings Settings { get; set; }

        public Mother3Rom(ISource source, Mother3RomSettings settings) : base(source)
        {
            Settings = settings;
            Initialize();
        }

        private void Initialize()
        {
            Version = DetectVersion();
        }

        private Mother3Version DetectVersion()
        {
            if (Header.Title != "MOTHER3" || Header.GameCode != "A3UJ")
                return Mother3Version.Invalid;

            if (Source.Length != 32 * 1024 * 1024)
                return Mother3Version.Invalid;

            // Get the SHA256
            string sha256 = Source.ComputeSHA256(0, Source.Length);

            switch (sha256)
            {
                case "d0f76a5a75454793f0ad59ee7b7b019a7af9ee3174cf0151439ba3ef00f73f98":
                    return Mother3Version.Japanese;

                case "46bc10ce8b28ccc4221568e942a049e558bbae1d94d66eda577ebd7c404f9f30":
                    return Mother3Version.English10;

                case "5480bc6098301825d12b9d2ef3280644e47fd7514402f5f6102de8e02d26b825":
                    return Mother3Version.English11;

                case "0c88dc774943ddd64ac6bfb1c0dc561b2e0146f07a1d5751bd9de73407d9b1f9":
                    return Mother3Version.English12;

                default:
                    return Mother3Version.None;
            }
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
