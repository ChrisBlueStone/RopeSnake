using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RopeSnake.Gba;

namespace RopeSnake.Mother3.IO
{
    public interface IMother3Reader : IGbaReader
    {
        FixedTableHeader ReadFixedTableHeader();
        Bg ReadBg();
        string ReadDialogString();
        string ReadSimpleString(int maxLength);
    }
}
