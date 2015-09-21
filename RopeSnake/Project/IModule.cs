using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.Project
{
    public interface IModule
    {
        void ReadJsonFiles(string projectDirectory);
        void WriteJsonFiles(string projectDirectory);
    }
}
