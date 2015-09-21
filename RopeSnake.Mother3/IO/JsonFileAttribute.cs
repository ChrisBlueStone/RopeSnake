using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.Mother3.IO
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class JsonFileAttribute : Attribute
    {
        private string fileName;

        public JsonFileAttribute(string fileName)
        {
            this.fileName = fileName;
        }
    }
}
