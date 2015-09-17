using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.Mother3.Data;

namespace RopeSnake.Mother3.IO
{
    public interface IMother3Data
    {
        Item ReadItem(int index);
    }
}
