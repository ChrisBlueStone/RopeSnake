using System.Collections.Generic;
using System.IO;
using RopeSnake.Mother3.Text;
using RopeSnake.IO;

namespace RopeSnake.Mother3.Sample
{
    class Program
    {
        static Dictionary<Mother3Version, string> romNames;

        static void Main(string[] args)
        {
            romNames = new Dictionary<Mother3Version, string>()
            {
                [Mother3Version.Japanese] = "mother3_jp",
                [Mother3Version.English10] = "mother3_en_v10",
                [Mother3Version.English11] = "mother3_en_v11",
                [Mother3Version.English12] = "mother3_en_v12"
            };

            Mother3Version currentVersion = Mother3Version.English10;

            ByteArraySource source = new ByteArraySource(romNames[currentVersion] + ".gba");

            RomSettings settings;
            if (currentVersion == Mother3Version.Japanese)
            {
                settings = new RomSettings(Path.Combine("Settings", romNames[currentVersion] + ".json"));
            }
            else
            {
                settings = new RomSettings(Path.Combine("Settings", "mother3_jp.json"),
                    Path.Combine("Settings", romNames[currentVersion] + ".json"));
            }

            Mother3Rom rom = new Mother3Rom(source, settings);

            TextModule text = new TextModule(rom);
            text.WriteModule(romNames[currentVersion]);
        }
    }
}
