using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.Mother3
{
    public class ScriptEncodingParameters
    {
        public int EvenPadAddress { get; set; }
        public int EvenPadModulus { get; set; }
        public int EvenOffset1 { get; set; }
        public int EvenOffset2 { get; set; }

        public int OddPadAddress { get; set; }
        public int OddPadModulus { get; set; }
        public int OddOffset1 { get; set; }
        public int OddOffset2 { get; set; }
    }
}
