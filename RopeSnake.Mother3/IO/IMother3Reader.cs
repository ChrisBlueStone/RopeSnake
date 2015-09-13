﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.Gba;
using RopeSnake.Mother3.RomStructures;

namespace RopeSnake.Mother3.IO
{
    public interface IMother3Reader : IGbaReader
    {
        Bg ReadBg();
    }
}