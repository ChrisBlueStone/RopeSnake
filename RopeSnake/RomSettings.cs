using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake
{
    public class RomSettings
    {
        protected Dictionary<string, object> settings;
         
        public T Get<T>(string name)
        {
            return (T)settings[name];
        }

        public void Set<T>(string name, T value)
        {
            settings[name] = value;
        }
    }
}
