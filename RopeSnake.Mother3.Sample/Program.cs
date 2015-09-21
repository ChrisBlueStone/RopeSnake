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

namespace RopeSnake.Mother3.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            /*ByteArraySource source = new ByteArraySource("mother3_jp.gba");
            RomSettings settings = new RomSettings("mother3_jp.json");*/

            ByteArraySource source = new ByteArraySource("mother3_en_v12.gba");
            RomSettings settings = new RomSettings("mother3_en_v12.json");
            Mother3Rom rom = new Mother3Rom(source, settings);

            DataModule data = new DataModule(rom);
            data.WriteModule("project");

            TextModule text = new TextModule(rom);
            text.WriteModule("project");
        }
    }
}
