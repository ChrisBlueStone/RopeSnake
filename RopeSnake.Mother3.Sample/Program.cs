using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using RopeSnake.Mother3;
using RopeSnake.Mother3.IO;
using RopeSnake.Mother3.Data;
using RopeSnake.Graphics;
using RopeSnake.Gba;
using RopeSnake.IO;

namespace RopeSnake.Mother3.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            ByteArraySource source = new ByteArraySource("mother3_jp.gba");
            Mother3RomSettings settings = Mother3RomSettings.FromYaml("mother3_jp.yml");
            Mother3Rom rom = new Mother3Rom(source, settings);
            Mother3RomReader reader = new Mother3RomReader(rom);
            Item mysticalStick = reader.ReadItem(12);
        }
    }
}
