using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace RopeSnake.Mother3.IO
{
    public class TableInfo
    {
        public int Address { get; set; }
        public int Count { get; set; }
        public int EntryLength { get; set; }
    }
}
