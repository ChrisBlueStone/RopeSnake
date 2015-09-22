using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RopeSnake.Mother3.Text
{
    public class ControlCode
    {
        public short Code { get; set; }
        public int Arguments { get; set; }
        public string Description { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Tag { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ControlCodeFlags Flags { get; set; }
    }

    [Flags]
    public enum ControlCodeFlags
    {
        None = 0,
        Terminate = 1
    }
}
