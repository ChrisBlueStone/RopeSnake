using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeSnake.IO
{
    public class SourceReader
    {
        public Source Source { get; protected set; }

        public virtual int Position { get; protected set; }

        protected SourceReader(Source source)
        {
            Source = source;
        }

        public virtual byte ReadByte() => ReadByte(Position++);

        public virtual byte ReadByte(int offset)
        {
            return Source.ReadByte(offset);
        }

        public virtual void WriteByte(byte value) => WriteByte(Position++, value);

        public virtual void WriteByte(int offset, byte value)
        {
            Source.WriteByte(offset, value);
        }

        public virtual void Seek(int offset, bool fromCurrent)
        {
            if (fromCurrent)
                Position += offset;
            else
                Position = offset;
        }
    }
}
