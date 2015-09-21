using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.Project
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ModuleFileAttribute : Attribute
    {
        private string fileName;

        public ModuleFileAttribute(string fileName)
        {
            this.fileName = fileName;
        }
    }
}
