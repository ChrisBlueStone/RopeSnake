using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using RopeSnake.Mother3;
using RopeSnake.Mother3.IO;
using RopeSnake.Mother3.Data;
using RopeSnake.Mother3.Text;
using RopeSnake.Graphics;
using RopeSnake.Gba;
using RopeSnake.IO;
using Newtonsoft.Json;

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

            Mother3Version currentVersion = Mother3Version.English12;

            ByteArraySource source = new ByteArraySource(romNames[currentVersion] + ".gba");
            RomSettings settings = new RomSettings(romNames[currentVersion] + ".json");
            Mother3Rom rom = new Mother3Rom(source, settings);

            TextModule text = new TextModule(rom);
            text.WriteModule(romNames[currentVersion]);
        }
    }
}
